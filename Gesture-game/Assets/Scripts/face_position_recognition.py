import mediapipe as mp
import cv2
from mediapipe.tasks import python
from mediapipe.tasks.python import vision
from mediapipe import solutions
from mediapipe.framework.formats import landmark_pb2
import numpy as np
import matplotlib.pyplot as plt
import socket
import time

host, port = "127.0.0.1", 25001
sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)

def draw_landmarks_on_image(rgb_image, detection_result):
    face_landmarks_list = detection_result.face_landmarks
    annotated_image = np.copy(rgb_image)

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
semaphore = False


def show_result(result: FaceLandmarkerResult, output_image: mp.Image, timestamp_ms: int):
    global detection_result
    global detection_result_available
    global semaphore
    if semaphore == False:
        detection_result = result
        detection_result_available = True

options = FaceLandmarkerOptions(
    base_options=BaseOptions(model_asset_path=model_path),
    running_mode=VisionRunningMode.LIVE_STREAM,
    result_callback=show_result)

with FaceLandmarker.create_from_options(options) as landmarker:
    videoDevice = cv2.VideoCapture(0)
    frameWidth = videoDevice.get(cv2.CAP_PROP_FRAME_WIDTH)
    frameHeight = videoDevice.get(cv2.CAP_PROP_FRAME_HEIGHT)

    sock.connect((host, port))

    frame_timestamp_ms = 0
    while videoDevice.isOpened():
        frame = videoDevice.read()[1]

        mp_image = mp.Image(image_format=mp.ImageFormat.SRGB, data=frame)

        frame_timestamp_ms = frame_timestamp_ms + 40

        landmarker.detect_async(mp_image, frame_timestamp_ms)
        if detection_result_available == True:
            annotated_image = draw_landmarks_on_image(mp_image.numpy_view(), detection_result)
            if len(detection_result.face_landmarks) > 0:
                semaphore = True
                #Point A
                annotated_image = cv2.circle(annotated_image, (
                    round(detection_result.face_landmarks[0][33].x * frameWidth),
                    round(detection_result.face_landmarks[0][33].y * frameHeight)), 5, (255, 0, 0), 2)
                #Point B
                annotated_image = cv2.circle(annotated_image, (
                    round(detection_result.face_landmarks[0][263].x * frameWidth),
                    round(detection_result.face_landmarks[0][263].y * frameHeight)), 5, (255, 0, 0), 2)
                #Point C
                annotated_image = cv2.circle(annotated_image, (
                    round(detection_result.face_landmarks[0][61].x * frameWidth),
                    round(detection_result.face_landmarks[0][61].y * frameHeight)), 5, (255, 0, 0), 2)
                #Point D
                annotated_image = cv2.circle(annotated_image, (
                    round(detection_result.face_landmarks[0][291].x * frameWidth),
                    round(detection_result.face_landmarks[0][291].y * frameHeight)), 5, (255, 0, 0), 2)
                semaphore = False

                cv2.imshow('Frame', annotated_image)

                z = - detection_result.face_landmarks[0][263].x + detection_result.face_landmarks[0][33].x #offset

                dataPoints = f"""{detection_result.face_landmarks[0][33].x}, {detection_result.face_landmarks[0][33].y},{detection_result.face_landmarks[0][33].z + z},
                                 {detection_result.face_landmarks[0][263].x},{detection_result.face_landmarks[0][263].y}, {detection_result.face_landmarks[0][263].z + z},
                                 {detection_result.face_landmarks[0][61].x}, {detection_result.face_landmarks[0][61].y},{detection_result.face_landmarks[0][61].z + z}, 
                                 {detection_result.face_landmarks[0][291].x},{detection_result.face_landmarks[0][291].y}, {detection_result.face_landmarks[0][291].z + z}"""

                sock.sendall(dataPoints.encode("utf-8"))

            if cv2.waitKey(5) & 0xFF == 27:
                break

    sock.close()
    videoDevice.release()
    cv2.destroyAllWindows()
