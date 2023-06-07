import sys
print(sys.executable)
import UnityEngine
import mediapipe as mp
import cv2
from mediapipe.tasks import python
from mediapipe.tasks.python import vision
from mediapipe import solutions
from mediapipe.framework.formats import landmark_pb2
import numpy as np
import matplotlib.pyplot as plt

def draw_landmarks_on_image(rgb_image, detection_result):
  face_landmarks_list = detection_result.face_landmarks
  annotated_image = np.copy(rgb_image)
  
  # Loop through the detected faces to visualize.
  for idx in range(len(face_landmarks_list)):
    face_landmarks = face_landmarks_list[idx]

    # Draw the face landmarks.
    face_landmarks_proto = landmark_pb2.NormalizedLandmarkList()
    face_landmarks_proto.landmark.extend([
      landmark_pb2.NormalizedLandmark(x=landmark.x, y=landmark.y, z=landmark.z) for landmark in face_landmarks
    ])

    solutions.drawing_utils.draw_landmarks(
        image=annotated_image,
        landmark_list=face_landmarks_proto,
        connections=mp.solutions.face_mesh.FACEMESH_TESSELATION,
        landmark_drawing_spec=None,
        connection_drawing_spec=mp.solutions.drawing_styles
        .get_default_face_mesh_tesselation_style())
    solutions.drawing_utils.draw_landmarks(
        image=annotated_image,
        landmark_list=face_landmarks_proto,
        connections=mp.solutions.face_mesh.FACEMESH_CONTOURS,
        landmark_drawing_spec=None,
        connection_drawing_spec=mp.solutions.drawing_styles
        .get_default_face_mesh_contours_style())
    solutions.drawing_utils.draw_landmarks(
        image=annotated_image,
        landmark_list=face_landmarks_proto,
        connections=mp.solutions.face_mesh.FACEMESH_IRISES,
          landmark_drawing_spec=None,
          connection_drawing_spec=mp.solutions.drawing_styles
          .get_default_face_mesh_iris_connections_style())
  return annotated_image


model_path = 'face_landmarker.task'

BaseOptions = mp.tasks.BaseOptions
FaceLandmarker = mp.tasks.vision.FaceLandmarker
FaceLandmarkerOptions = mp.tasks.vision.FaceLandmarkerOptions
FaceLandmarkerResult = mp.tasks.vision.FaceLandmarkerResult
VisionRunningMode = mp.tasks.vision.RunningMode

detection_result_available = False
detection_result = object()

# Create a face landmarker instance with the live stream mode:
def show_result(result: FaceLandmarkerResult, output_image: mp.Image, timestamp_ms: int):
  global detection_result
  global detection_result_available
  detection_result = result
  detection_result_available = True

options = FaceLandmarkerOptions(
    base_options=BaseOptions(model_asset_path=model_path),
    running_mode=VisionRunningMode.LIVE_STREAM,
    result_callback=show_result)

with FaceLandmarker.create_from_options(options) as landmarker:
  # The landmarker is initialized. Use it here.
  # ...
  
  # Use OpenCV’s VideoCapture to start capturing from the webcam.
  videoDevice = cv2.VideoCapture(0)

  # Create a loop to read the latest frame from the camera using VideoCapture#read()
  frame_timestamp_ms = 0
  while videoDevice.isOpened():
    frame = videoDevice.read()[1]
    
    # Convert the frame received from OpenCV to a MediaPipe’s Image object.
    mp_image = mp.Image(image_format=mp.ImageFormat.SRGB, data=frame)

    # Send live image data to perform face landmarking.
    # The results are accessible via the `result_callback` provided in
    # the `FaceLandmarkerOptions` object.
    # The face landmarker must be created with the live stream mode.
    #frame_timestamp_ms =  #videoDevice.get(cv2.CAP_PROP_POS_MSEC)
    #print(frame_timestamp_ms)
    frame_timestamp_ms = frame_timestamp_ms + 40
    
    landmarker.detect_async(mp_image, frame_timestamp_ms)
	
    if detection_result_available == True:
      annotated_image = draw_landmarks_on_image(mp_image.numpy_view(), detection_result)
      cv2.imshow('Frame', annotated_image)
      FACIAL_KEYPOINTS = mp.solutions.face_detection.FaceKeyPoint
      print(dir(FACIAL_KEYPOINTS))
      #print(len(detection_result.face_landmarks[0][]))
      #exit()
    #cv2.imshow('Video Preview', frame)
    if cv2.waitKey(5) & 0xFF == 27:
      break

  videoDevice.release()
  cv2.destroyAllWindows()