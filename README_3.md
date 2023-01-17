# AWS Lista 2

## Wykonaj backup bazy danych używany przez aplikację do S3
Zgodnie z intrukcją na stronie [Amazon RDS Backup & Restore Using AWS Backup](https://aws.amazon.com/getting-started/hands-on/amazon-rds-backup-restore-using-aws-backup/) wykonałem krok 2, czyli stworzyłem on-demand AWS Backup job dla bazy danych AWS RDS stworzonej dla aplikacji.

Backup został aktywowany w trybie natychmiastowym (czas wykonania 1h), natomiast retencja została ustawiona na 5 tygodni.

![Backup RDS](.markdown/Screenshot_20230115_152626.png)

![Backup RDS](.markdown/Screenshot_20230115_153133.png)

Po wykonaniu backupu pojawia się informacja o statusie "Completed" dla danego job'a w usłudze AWS Backup.

![Backup RDS](.markdown/Screenshot_20230115_170219.png)

## Użyj zapisanego backupu bazy danych, aby przywrócić dane do bazy danych używanej przez aplikację

W usłudze AWS Backup widoczne są dostępne punkty przywracania, gdzie w tym przypadku pierwszym punktem przywracania jest snapshot wykonany manualnie przeze mnie.

![Restore point](.markdown/Screenshot_20230115_170527.png)

Po wykonaniu polecenia przywracania bazy danych ze snapshotu pojawia się nowy job w zakładce `Restore jobs`.

![DB restore](.markdown/Screenshot_20230115_171853.png)

Po wykonaniu przywrócenia przez AWS widoczny jest status `completed`.

![DB restore](.markdown/Screenshot_20230115_173911.png)

Poniżej wydoczna jest aplikacja korzystająca z przywróconych danych aplikacji. Nadal ma ona dostępne stare dane aplikacji co widoczne jest po wejściu na `Leaderboard`.

![DB restore](.markdown/Screenshot_20230115_195834.png)

![DB restore](.markdown/Screenshot_20230115_195851.png)

## Utwórz konfigurację dla Cloudwatch a następnie zdefiniuj własny alarm

Korzystając z AWS CloudWatch stworzyłem alarm dla wykorzystania CPU w klastrze ECS dla środowiska aplikacji webowej. Jako threshold alarmu ustawiłem 50%.

![Cloudwatch](.markdown/Screenshot_20230117_134211.png)

Poniżej jest widoczna definicja tematu SNS dla alarmu.

![Cloudwatch](.markdown/Screenshot_20230117_134320.png)

![Cloudwatch](.markdown/Screenshot_20230117_134454.png)

![Cloudwatch SNS](.markdown/Screenshot_20230117_134645.png)

![Cloudwatch](.markdown/Screenshot_20230117_134803.png)

![CloudWatch Cpu](.markdown/Screenshot_20230117_134736.png)

Skonfigurowany alarm ma ustawione emaile, na które ma wysyłać emaile w przypadku wystąpienia sytuacji alarmowej.

## Utwórz konfigurację dla CloudTrail oraz sprawdź historię eventów

W usłudze CloudTrail włączyłem logowanie dla domyślnych eventów, a dodatkowo włączyłem `CloudTrail Insights` aby logowało anomalie związane z API rate'ami.

![CloudTrail](.markdown/Screenshot_20230115_174853.png)

W usłudze S3 pojawiły się foldery na logi z usługi CloudTrail.

![CloudTrail](.markdown/Screenshot_20230115_174945.png)

![CloudTrail](.markdown/Screenshot_20230115_190801.png)

![CloudTrail](.markdown/Screenshot_20230115_190943.png)

Każdy jeden plik jest skompresowany, ale można go podejrzeć w przeglądarce w formacie JSON.

![CloudTrail](.markdown/Screenshot_20230115_191056.png)

Poniżej widoczne są eventy CloudTrail, między innymi ustawianie alertów, oraz logowanie do konsoli.

![CloudTrail](.markdown/Screenshot_20230117_140128.png)

## Wykorzystaj AWS Inspector w swojej aplikacji

Próbując włączyć AWS Inspector na koncie studenckim AWS Academy natrafiłem na błąd uprawnień:

![AWS Inspector](.markdown/Screenshot_20230115_160441.png)

Ale po przełączeniu na wersję klasyczną udało się uruchomić tę usługę.

![Classic AWS Inspector](.markdown/Screenshot_20230115_173132.png)

![AWS Inspector started](.markdown/Screenshot_20230115_173148.png)

Poniżej widoczna jest informacja o włączeniu badania potencjalnych problemów z aplikacjami. Statud jest `running`.

![AWS Inspector](.markdown/Screenshot_20230115_175209.png)

Po wykonaniu badania AWS Inspector udostępnia listę znalezionych problemów związanymi z konfiguracją aplikacji oraz ich bezpieczeństwem.

![AWS Inspector](.markdown/Screenshot_20230115_190631.png)