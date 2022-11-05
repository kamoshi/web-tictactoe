# TicTacToe

## Stos technologiczny
- Baza danych: PostgreSQL (Alpine latest)
- Backend: ASP.NET Core 6
- Frontend: Angular + Nginx

W relacyjnej bazie danych trzymane są informacje dotyczące poszczególnych gier pomiędzy parami graczy, a także informacje o poszczególnych graczach. Aplikacja umożliwia tworzenie pojedynków pomiędzy losowymi parami graczy. Gracze są łączenie w pary podczas otwierania okna gry, a aplikacja wykorzystuje do działania endpointy REST udostępnione przez backend.

W obecnej wersji aplikacji pominięto konfigurację CORS oraz SSL.

## Docker

### Frontend

Do budowania aplikacji Angular wykorzystywany jest obraz `node:alpine`. Zbudowana aplikacja jest następnie kopiowana do konteneru bazującego na obrazie `nginx:alpine`, która udostępnia Nginx.  Aplikacja frontendowa jest aplikacją SPA, która korzysta z routera do wyświetlania odpowiednich stron w przeglądarce. Domyślna konfiguracja Nginx wyświetla aplikacje tylko pod adresem `/` co powoduje błędy podczas odświeżania stron. Błąd ten naprawiono aktualizując konfigurację Nginx w Dockerfile.

### Backend

Wykorzystano domyślny Dockerfile dla projektu ASP.NET Core 6.

### Baza danych

Baza danych jest konfigurowana tylko przez docker-compose-yml.

## AWS

Do konfiguracji AWS wykorzystano poradnik dostępny pod adresem:
https://aws.amazon.com/blogs/containers/deploy-applications-on-amazon-ecs-using-docker-compose/

```bash
kamov@msi:~$ docker context create ecs awstictactoe
"docker context create" requires exactly 1 argument.
See 'docker context create --help'.

Usage:  docker context create [OPTIONS] CONTEXT

Create a context
```

W tym przypadku okazało się, że tworzenie contextów związanych z ECS wymaga innej wersji Docker Compose CLI. Domyślny Docker nie obsługuje takich kontekstów.
https://docs.docker.com/cloud/ecs-integration/#install-the-docker-compose-cli-on-linux

```
kamov@msi:~$ curl -L https://raw.githubusercontent.com/docker/compose-cli/main/scripts/install/install_linux.sh | sh
  % Total    % Received % Xferd  Average Speed   Time    Time     Time  Current
                                 Dload  Upload   Total   Spent    Left  Speed
100  5982  100  5982   Running checks...    0 --:--:-- --:--:-- --:--:--     0
 0     0  10773      0 --:--:-- --:--:-- --:--:-- 10778
Checks passed!
Downloading CLI...
Downloaded CLI!
Installing CLI...
[sudo] password for kamov: 
Done!
```

```
kamov@msi:~$ docker context create ecs awstictactoe
? Create a Docker context using:  [Use arrows to move, type to filter]
> AWS secret and token credentials
  AWS environment variables
```

W tym momencie wyszukałem poradnik jak skonfigurować środowisko:
https://docs.docker.com/cloud/ecs-integration/
https://docs.aws.amazon.com/powershell/latest/userguide/pstools-appendix-sign-up.html

Wszedłem na stronę aby stworzyć Acess Key oraz Access Key Secret:
https://us-east-1.console.aws.amazon.com/iamv2/home?region=us-east-1#/home
https://us-east-1.console.aws.amazon.com/iamv2/home?region=us-east-1#/users

Okazało się, że lista jest pusta więc zdecydowałem się stworzyć nowego użytkownika o nazwie `docker-cli`.

Na podstawie strony https://docs.docker.com/cloud/ecs-integration/ spróbowałem stworzyć Policy dla nowego użytkownika:
```json
{
    "Version": "2012-10-17",
    "Statement": [
        {
            "Effect": "Allow",
            "Action": [
                "application-autoscaling:*",
                "cloudformation:*",
                "ec2:AuthorizeSecurityGroupIngress",
                "ec2:CreateSecurityGroup",
                "ec2:CreateTags",
                "ec2:DeleteSecurityGroup",
                "ec2:DescribeRouteTables",
                "ec2:DescribeSecurityGroups",
                "ec2:DescribeSubnets",
                "ec2:DescribeVpcs",
                "ec2:RevokeSecurityGroupIngress",
                "ecs:CreateCluster",
                "ecs:CreateService",
                "ecs:DeleteCluster",
                "ecs:DeleteService",
                "ecs:DeregisterTaskDefinition",
                "ecs:DescribeClusters",
                "ecs:DescribeServices",
                "ecs:DescribeTasks",
                "ecs:ListAccountSettings",
                "ecs:ListTasks",
                "ecs:RegisterTaskDefinition",
                "ecs:UpdateService",
                "elasticloadbalancing:*",
                "iam:AttachRolePolicy",
                "iam:CreateRole",
                "iam:DeleteRole",
                "iam:DetachRolePolicy",
                "iam:PassRole",
                "logs:CreateLogGroup",
                "logs:DeleteLogGroup",
                "logs:DescribeLogGroups",
                "logs:FilterLogEvents",
                "route53:CreateHostedZone",
                "route53:DeleteHostedZone",
                "route53:GetHealthCheck",
                "route53:GetHostedZone",
                "route53:ListHostedZonesByName",
                "servicediscovery:*"
            ],
            "Resource": "*"
        }
    ]
}
```

Okazało się, że nie jestem w stanie stworzyć użytkownika, ponieważ nie mam permisji `iam:CreateUser`.
```
Unable to create user
AWS could not create the user you requested. Learn more
User: arn:aws:sts::818571164127:assumed-role/voclabs/user2254751=Maciej_Jur is not authorized to perform: iam:CreateUser on resource: arn:aws:iam::818571164127:user/docker-cli because no identity-based policy allows the iam:CreateUser action
```

Zainstalowałem więc AWS CLI, a także ustawiłem zmienne w `~/.aws/credentials`.
```
kamov@msi:~$ sudo ./aws/install
[sudo] password for kamov: 
You can now run: /usr/local/bin/aws --version
kamov@msi:~$ aws

usage: aws [options] <command> <subcommand> [<subcommand> ...] [parameters]
To see help text, you can run:

  aws help
  aws <command> help
  aws <command> <subcommand> help

aws: error: the following arguments are required: command

kamov@msi:~$ aws configure
AWS Access Key ID [****************W5XU]:
AWS Secret Access Key [****************oRRa]:
Default region name [None]: us-east-1
Default output format [None]: json
kamov@msi:~$ docker context create ecs awstictactoe
? Create a Docker context using: An existing AWS profile
? Select AWS Profile default
Successfully created ecs context "awstictactoe"
```

Następnie spróbowałem zbudować aplikację z Docker compose:
```
kamov@msi:~/Desktop/tictactoe (main)$ docker context use awstictactoe
awstictactoe
kamov@msi:~/Desktop/tictactoe (main)$ docker context ls 
NAME                TYPE                DESCRIPTION                               DOCKER ENDPOINT               KUBERNETES ENDPOINT   ORCHESTRATOR
awstictactoe *      ecs                                                                                                               
default             moby                Current DOCKER_HOST based configuration   unix:///var/run/docker.sock                         swarm
kamov@msi:~/Desktop/tictactoe (main)$ docker compose up
WARNING services.build: unsupported attribute        
service frontend doesn't define a Docker image to run: incompatible attribute
```

Spróbowałem naprawić błąd na podstawie tego poradnika:
https://www.docker.com/blog/docker-compose-from-local-to-amazon-ecs/

Stworzyłem konto na Docker Hub, a następnie spushowałem obrazy.
```
kamov@msi:~/Desktop/tictactoe (main)$ docker compose build
[+] Building 65.8s (35/35) FINISHED                                                                                                                            ...

Use 'docker scan' to run Snyk tests against images to find vulnerabilities and learn how to fix them
kamov@msi:~/Desktop/tictactoe (main)$ docker login
Login with your Docker ID to push and pull images from Docker Hub. If you don't have a Docker ID, head over to https://hub.docker.com to create one.
Username: kamov
Password: 
WARNING! Your password will be stored unencrypted in /home/kamov/.docker/config.json.
Configure a credential helper to remove this warning. See
https://docs.docker.com/engine/reference/commandline/login/#credentials-store

Login Succeeded

Logging in with your password grants your terminal complete access to your account. 
For better security, log in with a limited-privilege personal access token. Learn more at https://docs.docker.com/go/access-tokens/
kamov@msi:~/Desktop/tictactoe (main)$ docker compose push
[+] Running 1/18
 ⠿ database Skipped                                                                                                                            ...
kamov@msi:~/Desktop/tictactoe (main)$ 
```

Spróbowałem wywołać `docker compose up` ale pokazał się błąd związany z mapowanie portów w kontenerach:
```
kamov@msi:~/Desktop/tictactoe (main)$ docker compose up
WARNING services.build: unsupported attribute        
published port can't be set to a distinct value than container port: incompatible attribute
```


