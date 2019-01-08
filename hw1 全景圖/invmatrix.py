import numpy as np
matrix=np.array([[380,293,1,0,0,0],
                 [0,0,0,380,293,1],
                 [380,437,1,0,0,0],
                 [0,0,0,380,437,1],
                 [1146,200,1,0,0,0],
                 [0,0,0,1146,200,1],],dtype=float)
b=np.array([[310],
            [235],
            [310],
            [383],
            [1041],
             [156],],dtype=float)
c=np.array([70,140,78,140,78,147],dtype=float)
a=np.linalg.inv(matrix)
print (a.tolist())
print ("a-1 dot b")
print(np.matmul(a,b).tolist())
print ("/////////////////////")