#!/bin/bash

docker build . -t ttt-backend
docker tag ttt-backend:latest 818571164127.dkr.ecr.us-east-1.amazonaws.com/ttt-backend:latest
docker push 818571164127.dkr.ecr.us-east-1.amazonaws.com/ttt-backend:latest
