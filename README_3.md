# AWS Lista 2

## Wykonaj backup bazy danych używany przez aplikację do S3
Zgodnie z intrukcją na stronie [Amazon RDS Backup & Restore Using AWS Backup](https://aws.amazon.com/getting-started/hands-on/amazon-rds-backup-restore-using-aws-backup/) wykonałem krok 2, czyli stworzyłem on-demand AWS Backup job dla bazy danych AWS RDS stworzonej dla aplikacji.

Backup został aktywowany w trybie natychmiastowym (czas wykonania 1h), natomiast retencja została ustawiona na 5 tygodni.

![Backup RDS](.markdown/Screenshot_20230115_152626.png)

![Backup RDS](.markdown/Screenshot_20230115_153133.png)

![Backup RDS](.markdown/Screenshot_20230115_170219.png)

## Użyj zapisanego backupu bazy danych, aby przywrócić dane do bazy danych używanej przez aplikację

![Restore point](.markdown/Screenshot_20230115_170527.png)

![DB restore](.markdown/Screenshot_20230115_171853.png)

![DB restore](.markdown/Screenshot_20230115_173911.png)

![DB restore](.markdown/Screenshot_20230115_195834.png)

![DB restore](.markdown/Screenshot_20230115_195851.png)

## Utwórz konfigurację dla Cloudwatch a następnie zdefiniuj własny alarm

Korzystając z AWS CloudWatch stworzyłem alarm dla wykorzystania CPU w klastrze ECS dla środowiska aplikacji webowej. Jako threshold alarmu ustawiłem 50%.

![CloudWatch Cpu](.markdown/Screenshot_20230115_155345.png)

## Utwórz konfigurację dla CloudTrail oraz sprawdź historię eventów

![CloudTrail](.markdown/Screenshot_20230115_174853.png)

![CloudTrail](.markdown/Screenshot_20230115_174945.png)

![CloudTrail](.markdown/Screenshot_20230115_190801.png)

![CloudTrail](.markdown/Screenshot_20230115_190943.png)

![CloudTrail](.markdown/Screenshot_20230115_191056.png)

## Wykorzystaj AWS Inspector w swojej aplikacji

Próbując włączyć AWS Inspector na koncie studenckim AWS Academy natrafiłem na błąd uprawnień:

![AWS Inspector](.markdown/Screenshot_20230115_160441.png)

Ale po przełączeniu na wersję klasyczną udało się uruchomić tę usługę.

![Classic AWS Inspector](.markdown/Screenshot_20230115_173132.png)

![AWS Inspector started](.markdown/Screenshot_20230115_173148.png)

![AWS Inspector](.markdown/Screenshot_20230115_175209.png)

![AWS Inspector](.markdown/Screenshot_20230115_190631.png)