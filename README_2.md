# AWS Lista 2

## Router
W swoim rozwiązaniu stosuje obraz routera, który mapuje odpowiednie adresy na porty frontendu oraz backendu. Jest to obraz bazujący na obrazie `nginx`, i ma następującą konfigurację:

```nginx
upstream ttt-web {
  server ttt-web:8080;
}

upstream ttt-api {
  server ttt-api:7135;
}

server {
  listen 80;
  
  location / {
    proxy_pass http://ttt-web;
  }
  
  location /api {
    proxy_pass http://ttt-api;
    proxy_set_header X-Forwarded-Proto $scheme;
  }
}
```

## Upload to ECR
Obrazy są budowane, tagowane i uploadowane do ECR.

W ECR tworze 2 repozytoria o nazwach `ttt-frontend` oraz `ttt-backend`, a następnie wywołuje komendy w `bash`:

Frontend:
```bash
docker build . -t ttt-frontend
docker tag ttt-frontend:latest 818571164127.dkr.ecr.us-east-1.amazonaws.com/ttt-frontend:latest
docker push 818571164127.dkr.ecr.us-east-1.amazonaws.com/ttt-frontend:latest
```

Backend:
```bash
docker build . -t ttt-backend
docker tag ttt-backend:latest 818571164127.dkr.ecr.us-east-1.amazonaws.com/ttt-backend:latest
docker push 818571164127.dkr.ecr.us-east-1.amazonaws.com/ttt-backend:latest
```

Router:
```bash
docker build . -t ttt-router
docker tag ttt-router:latest 818571164127.dkr.ecr.us-east-1.amazonaws.com/ttt-router:latest
docker push 818571164127.dkr.ecr.us-east-1.amazonaws.com/ttt-router:latest
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

## Konfiguracja ELB
Z poziomu środowiska Beanstalk wchodzę w konfigurację, a następnie zmieniam konfigurację:
- Capacity: zmiana opcji `single instance` na `load balanced`
  - instances min 1, max 2
  - fleet composition: on-demand instances
  - availability zones: any
- Load balancer:
  - mapuje port 80 na port 80 kontenerów

## Konfiguracja RDS
Do mojego środowiska Beanstalk dodaje bazędanych bazującą na Postgres:
- instance class: `db.t3.micro`
- storage: 10GB
- username: postgres
- hasło: postgres
- availability: low (one AZ)
- deletion policy: create snapshot

Po stworzeniu bazy danych uzyskuje endpoint pozwalający na połączenie się z bazą danych.

## Dockerrun.aws.json
Aby uruchomić swoją aplikację w usłudze Beanstalk tworzę plik `Dockerrun.aws.json` na podstawie pliku `docker-compose.yml`:

```json
{
  "AWSEBDockerrunVersion": 2,
  "containerDefinitions": [
    {
      "name": "ttt-api",
      "image": "818571164127.dkr.ecr.us-east-1.amazonaws.com/ttt-backend",
      "hostname": "ttt-api",
      "essential": true,
      "portMappings": [
        {
          "hostPort": 7135,
          "containerPort": 7135
        }
      ],
      "memory": 512,
      "environment": [
        {
          "name": "DB_CONNECTION_STRING",
          "value": "Host=awseb-e-uymamqabur-stack-awsebrdsdatabase-zov6cz0h9ais.cob11mf5uvvv.us-east-1.rds.amazonaws.com;Port=5432;Username=postgres;Password=postgres;Database=tictactoe"
        }
      ]
    },
    {
      "name": "ttt-web",
      "image": "818571164127.dkr.ecr.us-east-1.amazonaws.com/ttt-frontend",
      "hostname": "ttt-web",
      "essential": true,
      "portMappings": [
        {
          "hostPort": 8080,
          "containerPort": 8080
        }
      ],
      "memory": 256
    },
    {
      "name": "router",
      "image": "818571164127.dkr.ecr.us-east-1.amazonaws.com/ttt-router",
      "hostname": "router",
      "essential": true,
      "portMappings": [
        {
          "hostPort": 80,
          "containerPort": 80
        }
      ],
      "links": ["ttt-web", "ttt-api"],
      "memory": 128
    }
  ]
}
```

W pliku tym umieszczam connection string do bazy danych zawierający adres instancji bazy danych RDS utworzony wcześniej.


## Wdrożenie

Na głównym ekranie środowiska Beanstalk klikam `Upload and deploy` a nastepnie przesyłam plik `Dockerrun.aws.json`. Aplikacja zostaje wdrożone i jest dostępna pod adresem:

Tictactoe-env.eba-mxmp3x75.us-east-1.elasticbeanstalk.com 

