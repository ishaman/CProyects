CC=gcc
CFLAGS=
LDFLAGS=
OBJS= 

all:            mopsocd

mopsocd:	mopsohv.c test-fun.h randomlib.h indicadorHyoe.h
		$(CC) $(CFLAGS) $(LDFLAGS) -o mopsohv mopsohv.c $(OBJS) -lm

clean:
		rm *.o 

real-clean:          
		rm *~ *.o *.out
