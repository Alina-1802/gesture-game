import cv2
import mediapipe as mp
import RawData as rd
import pickle

videoDevice = cv2.VideoCapture(0)
width = videoDevice.get(cv2.CAP_PROP_FRAME_WIDTH)
height = videoDevice.get(cv2.CAP_PROP_FRAME_HEIGHT)

mp_drawing = mp.solutions.drawing_utils
mp_hands = mp.solutions.hands
#mp_drawing_styles = mp.solutions.drawing_styles
#DETECTION - POCZATKOWA, TRACKING - SLEDZACA / max_num_hands=4 zwieksza liczbe rak
hands = mp_hands.Hands(model_complexity=1,min_detection_confidence=0.5, min_tracking_confidence=0.5)

print('Place your hand, show the desired gesture and press ESC when ready...')

while videoDevice.isOpened():

	frame = videoDevice.read()[1]
	image = cv2.cvtColor(frame, cv2.COLOR_BGR2RGB)
	image.flags.writeable = False
	results = hands.process(image)
	image.flags.writeable = True

	if results.multi_hand_landmarks:
		for num, hand in enumerate(results.multi_hand_landmarks):
			mp_drawing.draw_landmarks(frame, hand, mp_hands.HAND_CONNECTIONS, 
			mp_drawing.DrawingSpec(color=(121,22,76),thickness=2,circle_radius=4),
			mp_drawing.DrawingSpec(color=(121,44,76),thickness=2,circle_radius=2))
			
	cv2.imshow('Video Preview', frame)
			
	if cv2.waitKey(5) & 0xFF == 27:
		break

rawData = rd.RawData(results, width, height)
with open("model", "wb") as outfile:
	pickle.dump(rawData, outfile)

videoDevice.release()
cv2.destroyAllWindows()