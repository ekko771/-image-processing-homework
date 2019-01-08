import os, cv2, numpy
import sys
def l_callbackFunc(e,x,y,f,p):
    if e==cv2.EVENT_LBUTTONDOWN:
        print(x,y)
        point["left"][0] = x
        point["left"][1] = y
cv2.namedWindow("Image")
cv2.setMouseCallback("Image", l_callbackFunc, None)
point = {"left": [-1,-1]}
img = cv2.imread("C:/Users/USER/Desktop/q2.jpg")   
cv2.imshow("Image", img)   
cv2.waitKey (0)  
cv2.destroyAllWindows()  