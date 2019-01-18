import clr
from time import sleep
clr.AddReference('urgDLLcs')
from urgDLLcs import urgClass

import numpy as np
import cv2
import math

test = urgClass("COM3", 115200)

def calc (data):
    # Create a black image
    img = np.zeros((1000,1000,3), np.uint8)
    if len(data) != 0:
        for i in range(len(data)):
            if i+1 < len(data) :
                cv2.line(img,data[i],data[i+1],(255,0,0),1)
        
        cv2.line(img,(500,500),data[len(data)-1],(255,255,0),1)
        cv2.line(img,(500,500),data[0],(255,255,255),1)

    return img

while True:
    test.run()
    point = []
    for i in range(800):
        degree = test.pulldata_degree(i)
        length =  test.pulldata_distance(i)
        
        #print degree
        #print length

        if degree != -1 and length != -1:
            length = length /5
            x = length * math.cos(math.radians(degree))
            y = length * math.sin(math.radians(degree))
            if abs(x) > 10 and abs(y) > 10:
                point.append((int(x+500),int(y+500)))
        #else:
            #point.append((300,300))

    cv2.imshow("a",calc(point))
    cv2.waitKey(1)



