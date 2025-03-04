import sys
import cv2
import os
import numpy as np
import tkinter as tk
from tkinter import filedialog
from PIL import Image, ImageTk

# Global Variables
selected_image_path = None

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

def select_image():
    """ Opens file dialog to select an image """
    global selected_image_path
    file_path = filedialog.askopenfilename(filetypes=[("Image Files", "*.jpg;*.jpeg;*.png;*.bmp")])

    if not file_path:
        return

    selected_image_path = file_path
    display_image(file_path, original_label)

def process_and_display():
    """ Enhances the selected image and displays the result """
    global selected_image_path

    if selected_image_path is None:
        result_label.config(text="Please select an image first!", fg="red")
        return

    # Read the selected image
    image = cv2.imread(selected_image_path)

    # Apply enhancement
    enhanced_image = enhance_image(image)

    # Save the processed image
    output_folder = os.path.join(os.getcwd(), "Image_Output")
    os.makedirs(output_folder, exist_ok=True)
    processed_image_path = os.path.join(output_folder, "enhanced_" + os.path.basename(selected_image_path))
    cv2.imwrite(processed_image_path, enhanced_image)

    # Display the enhanced image
    display_image(processed_image_path, enhanced_label)

    # Update result label
    result_label.config(text=f"Enhanced image saved to:\n{processed_image_path}", fg="green")

def display_image(image_path, label):
    """ Displays an image in a Tkinter label """
    image = Image.open(image_path)
    image = image.resize((300, 300), Image.LANCZOS)
    photo = ImageTk.PhotoImage(image)

    label.config(image=photo)
    label.image = photo

# Tkinter UI Setup
root = tk.Tk()
root.title("Image Enhancement Tester")
root.geometry("700x500")

# Upload Button
upload_btn = tk.Button(root, text="Upload Image", command=select_image, font=("Arial", 14), bg="lightblue")
upload_btn.pack(pady=10)

# Labels for displaying images
original_label = tk.Label(root, text="Original Image")
original_label.pack()
enhanced_label = tk.Label(root, text="Enhanced Image")
enhanced_label.pack()

# Process Button
process_btn = tk.Button(root, text="Enhance Image", command=process_and_display, font=("Arial", 14), bg="lightgreen")
process_btn.pack(pady=10)

# Result Label
result_label = tk.Label(root, text="", font=("Arial", 12))
result_label.pack()

# Start the GUI
root.mainloop()
