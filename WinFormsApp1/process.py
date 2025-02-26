import sys
import cv2
import os
import numpy as np

# Global Variables
manual_points = []
resized_image = None
original_image = None
original_image_path = None
ratio = 1  # Will be updated later

def order_points(pts):
    """ Orders the points in the order: top-left, top-right, bottom-right, bottom-left """
    rect = np.zeros((4, 2), dtype="float32")
    s = pts.sum(axis=1)
    diff = np.diff(pts, axis=1)
    rect[0] = pts[np.argmin(s)]  # Top-left
    rect[2] = pts[np.argmax(s)]  # Bottom-right
    rect[1] = pts[np.argmin(diff)]  # Top-right
    rect[3] = pts[np.argmax(diff)]  # Bottom-left
    return rect

def four_point_perspective_transform(image, pts):
    """ Performs a perspective transformation using the given four points """
    rect = order_points(pts)
    (tl, tr, br, bl) = rect

    widthA = np.linalg.norm(br - bl)
    widthB = np.linalg.norm(tr - tl)
    maxWidth = max(int(widthA), int(widthB))

    heightA = np.linalg.norm(tr - br)
    heightB = np.linalg.norm(tl - bl)
    maxHeight = max(int(heightA), int(heightB))

    dst = np.array([
        [0, 0],
        [maxWidth - 1, 0],
        [maxWidth - 1, maxHeight - 1],
        [0, maxHeight - 1]
    ], dtype="float32")

    M = cv2.getPerspectiveTransform(rect, dst)
    warped = cv2.warpPerspective(image, M, (maxWidth, maxHeight))

    return warped

def select_points(event, x, y, flags, param):
    global manual_points, resized_image, ratio

    if event == cv2.EVENT_LBUTTONDOWN:
        if len(manual_points) < 4:
            manual_points.append((x * ratio, y * ratio))  # Scale back to original image
            cv2.circle(resized_image, (x, y), 5, (0, 255, 0), -1)
            cv2.imshow("Select 4 Points", resized_image)

        if len(manual_points) == 4:
            cv2.destroyWindow("Select 4 Points")  # Close window after 4 points

def process_image(image_PATH):
    global ratio, resized_image, manual_points, original_image, original_image_path

    original_image_path = image_PATH  # Store path
    manual_points = []  # Reset selected points

    if not os.path.exists(image_PATH):
        print(f"Error: Image file '{image_PATH}' not found.", file=sys.stderr)
        return

    image = cv2.imread(image_PATH)
    if image is None:
        print(f"Error: Unable to read image '{image_PATH}'.", file=sys.stderr)
        return

    original_image = image.copy()

    height = 800
    ratio = image.shape[0] / float(height)
    resized_width = int(image.shape[1] / ratio)
    resized_image = cv2.resize(image, (resized_width, height))

    image_gray = cv2.cvtColor(resized_image, cv2.COLOR_BGR2GRAY)
    image_blurred = cv2.GaussianBlur(image_gray, (5, 5), 0)
    image_edge = cv2.Canny(image_blurred, 75, 200)

    cnts, _ = cv2.findContours(image_edge.copy(), cv2.RETR_LIST, cv2.CHAIN_APPROX_SIMPLE)
    cnts = sorted(cnts, key=cv2.contourArea, reverse=True)[:5]

    screenCnt = None
    for c in cnts:
        peri = cv2.arcLength(c, True)
        approx = cv2.approxPolyDP(c, 0.02 * peri, True)
        if len(approx) == 4:
            screenCnt = approx
            break

    if screenCnt is None:
        # print("Automatic detection failed. Please select 4 points manually.")
        cv2.imshow("Select 4 Points", resized_image)
        cv2.setMouseCallback("Select 4 Points", select_points)
        cv2.waitKey(0)  # Wait for manual selection to complete

        if len(manual_points) == 4:
            screenCnt = np.array(manual_points, dtype="float32")
        else:
            print("Error: Not enough points selected.", file=sys.stderr)
            return

    else:
        screenCnt = screenCnt.reshape(4, 2) * ratio

    screenCnt = order_points(screenCnt)
    warped_image = four_point_perspective_transform(original_image, screenCnt.astype(int))

    if warped_image is None or warped_image.size == 0:
        print("Error: The warped image is empty. Check the perspective transform function.", file=sys.stderr)
        return

    processed_image_path = os.path.join(os.path.dirname(image_PATH), "processed_" + os.path.basename(image_PATH))
    cv2.imwrite(processed_image_path, warped_image)

    cv2.imshow("Scanned Document", warped_image)
    cv2.waitKey(0)
    cv2.destroyAllWindows()

    print(processed_image_path)  # ✅ Output for C#

if __name__ == "__main__":
    if len(sys.argv) < 2:
        print("Error: No images provided.", file=sys.stderr)
        sys.exit(1)

    input_images = sys.argv[1:]
    for img in input_images:
        process_image(img)
