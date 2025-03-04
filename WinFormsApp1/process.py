import sys
import cv2
import os
import numpy as np
import tkinter as tk

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

def enhance_image(image):
    """ Enhances color, sharpness, and contrast """

    # Convert to LAB color space
    lab = cv2.cvtColor(image, cv2.COLOR_BGR2LAB)
    
    # Apply CLAHE to the L (lightness) channel
    l, a, b = cv2.split(lab)
    clahe = cv2.createCLAHE(clipLimit=3.0, tileGridSize=(8, 8))
    l = clahe.apply(l)

    # Merge back LAB channels and convert to BGR
    lab_enhanced = cv2.merge((l, a, b))
    enhanced = cv2.cvtColor(lab_enhanced, cv2.COLOR_LAB2BGR)

    # Increase color saturation by converting to HSV
    hsv = cv2.cvtColor(enhanced, cv2.COLOR_BGR2HSV)
    h, s, v = cv2.split(hsv)
    s = cv2.add(s, 30)  # Boost saturation
    s = np.clip(s, 0, 255)  # Ensure valid range
    hsv_enhanced = cv2.merge((h, s, v))

    # Convert back to BGR
    final_enhanced = cv2.cvtColor(hsv_enhanced, cv2.COLOR_HSV2BGR)

    return final_enhanced

#Threshold
def remove_shadows(warped_image):
    """Step 1: Removes shadows and converts the paper to pure white."""
    gray = cv2.cvtColor(warped_image, cv2.COLOR_BGR2GRAY)

    # Use morphological operations to estimate the background
    dilated_img = cv2.dilate(gray, np.ones((7, 7), np.uint8))  
    bg_img = cv2.medianBlur(dilated_img, 21)  

    # Remove shadows by subtracting background and normalizing
    diff_img = 255 - cv2.absdiff(gray, bg_img)
    norm_img = cv2.normalize(diff_img, None, 0, 255, cv2.NORM_MINMAX)

    # Apply adaptive thresholding to remove extreme lighting
    shadow_free = cv2.adaptiveThreshold(norm_img, 255, cv2.ADAPTIVE_THRESH_GAUSSIAN_C, 
                                        cv2.THRESH_BINARY, 21, 9)

    # Convert back to 3-channel grayscale
    shadow_free = cv2.cvtColor(shadow_free, cv2.COLOR_GRAY2BGR)

    return shadow_free


def restore_natural_paper_color(shadow_free):
    """Step 2: Adjusts the paper background to a natural beige/off-white color."""
    # Convert to LAB color space to manipulate brightness and color separately
    lab = cv2.cvtColor(shadow_free, cv2.COLOR_BGR2LAB)
    l, a, b = cv2.split(lab)

    # Adjust brightness (L) to make it softer
    l = cv2.normalize(l, None, 20, 255, cv2.NORM_MINMAX)  # Adjust the range for a softer white

    # Adjust the "b" channel (Blue-Yellow) to add a warm beige tone
    b = cv2.addWeighted(b, 1.2, np.full_like(b, 270), 0.3, 0)  # Fine-tune this value to match the paper color

    # Merge adjusted channels back and convert to BGR
    final_lab = cv2.merge((l, a, b))
    final_img = cv2.cvtColor(final_lab, cv2.COLOR_LAB2BGR)

    # Apply slight denoising to smooth out the paper texture
    final_img = cv2.fastNlMeansDenoisingColored(final_img, None, 10, 10, 7, 21)

    return final_img

def process_image(image_PATH, combobox_value):
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
    
    output_folder = os.path.join(os.getcwd(), "Image_Output")
    if combobox_value == 1:
        Final_Image = remove_shadows(warped_image)
        processed_image_path = os.path.join(output_folder, "BW_processed_" + os.path.basename(image_PATH))
    elif combobox_value ==2:
        Final_Image = enhance_image(warped_image)
        processed_image_path = os.path.join(output_folder, "color_enhanced_processed_" + os.path.basename(image_PATH))
    else:
        Final_Image = warped_image
        processed_image_path = os.path.join(output_folder, "processed_" + os.path.basename(image_PATH))
    
    
    cv2.imwrite(processed_image_path, Final_Image)
    cv2.imshow("Scanned Document", Final_Image)
    cv2.waitKey(0)
    cv2.destroyAllWindows()

    print(processed_image_path)  # ✅ Output for C#

def manual_shadow_removal_code(image_PATH):
    image = cv2.imread(image_PATH)
    shadrem = remove_shadows(image)
    output_folder = os.path.join(os.getcwd(), "WinFormsApp1", "Image_Output")
    processed_image_path = os.path.join(output_folder, "shadowed_" + os.path.basename(image_PATH))
    if shadrem is None:
        print("Error: remove_shadows() returned None", file=sys.stderr)
    else:
        if os.path.exists(output_folder):
            cv2.imwrite(processed_image_path, shadrem)
            print(f"Image saved successfully: {processed_image_path}")
        else:
            print(f"Folder {output_folder} does not exist")

if __name__ == "__main__":
    if len(sys.argv) < 3:  # At least 1 image + 1 combobox argument
        print("Error: No images provided or missing combobox value.", file=sys.stderr)
        sys.exit(1)

    try:
        # Read the first argument as the combobox value
        combobox_value = int(sys.argv[1])  
    except ValueError:
        print("Error: Invalid combobox value. It should be an integer.", file=sys.stderr)
        sys.exit(1)

    # Read the remaining arguments as image paths
    input_images = sys.argv[2:]  # Start from index 2 (skip script name and combobox)
    
    for img in input_images:
        process_image(img, combobox_value)