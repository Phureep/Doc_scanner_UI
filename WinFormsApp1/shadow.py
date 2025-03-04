import cv2
import numpy as np
import sys
import os

def remove_shadows(image_path):
    """Step 1: Removes shadows and converts the paper to pure white."""
    img = cv2.imread(image_path, cv2.IMREAD_COLOR)
    if img is None:
        print("Error: Could not load image.", file=sys.stderr)
        return None

    # Convert to grayscale
    gray = cv2.cvtColor(img, cv2.COLOR_BGR2GRAY)

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
    b = cv2.addWeighted(b, 0.9, np.full_like(b, 140), 0.1, 0)  # Fine-tune this value to match the paper color

    # Merge adjusted channels back and convert to BGR
    final_lab = cv2.merge((l, a, b))
    final_img = cv2.cvtColor(final_lab, cv2.COLOR_LAB2BGR)

    # Apply slight denoising to smooth out the paper texture
    final_img = cv2.fastNlMeansDenoisingColored(final_img, None, 10, 10, 7, 21)

    return final_img


def main():
    if len(sys.argv) < 2:
        print("Usage: python shadow.py <image_path>", file=sys.stderr)
        sys.exit(1)

    image_path = sys.argv[1]

    # Step 1: Remove Shadows
    shadow_free = remove_shadows(image_path)
    if shadow_free is None:
        sys.exit(1)

    # Step 2: Restore Paper Color
    final_result = restore_natural_paper_color(shadow_free)

    # Save the final image
    output_path = image_path.replace(".", "_final_natural_paper.")
    cv2.imwrite(output_path, final_result)

    print(output_path)  # Output the final file path for C# integration


if __name__ == "__main__":
    main()
