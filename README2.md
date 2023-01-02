# AWS Lista 2

## Upload to ECR
Obrazy są budowane, tagowane i uploadowane do ECR.

W ECR tworze 2 repozytoria o nazwach `ttt-frontend` oraz `ttt-backend`, a następnie wywołuje komendy w `bash`:
```bash
#!/bin/bash

docker build . -t ttt-frontend
docker tag ttt-frontend:latest 818571164127.dkr.ecr.us-east-1.amazonaws.com/ttt-frontend:latest
docker push 818571164127.dkr.ecr.us-east-1.amazonaws.com/ttt-frontend:latest
```

```bash
docker build . -t ttt-backend
docker tag ttt-backend:latest 818571164127.dkr.ecr.us-east-1.amazonaws.com/ttt-backend:latest
docker push 818571164127.dkr.ecr.us-east-1.amazonaws.com/ttt-backend:latest
```

Obrazy pojawiają się w usłudze ECR.

## Tworzenie aplikacji ElasticBeanstalk

Wyklikuje aplikację Docker uwzględniając następujące ustawienia zaawansowane:

```
AWS Elastic Beanstalk
• This service can assume the LabRole IAM role.
• To create an application: choose Create Application, give it an application name, choose a
platform, then choose Configure more options. Scroll down to the Security panel and choose
Edit. For Service role, choose LabRole. If the environment is in the us-east-1 AWS Region, for
EC2 key pair, choose vockey and for IAM instance profile, choose LabInstanceProfile. Choose
Save, then choose Create app.
• Supported instance types: nano, micro, small, medium, and large. If you attempt to launch a
larger instance type, it will be terminated.
```

Pod adresem http://ttt-env.eba-ptp2pdik.us-east-1.elasticbeanstalk.com/ dostępna jest przykładowa aplikacja typu Hello World.