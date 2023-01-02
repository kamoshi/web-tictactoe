#!/bin/bash

docker build . -t ttt-frontend
docker tag ttt-frontend:latest 818571164127.dkr.ecr.us-east-1.amazonaws.com/ttt-frontend:latest
docker push 818571164127.dkr.ecr.us-east-1.amazonaws.com/ttt-frontend:latest
