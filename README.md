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

TODO...
