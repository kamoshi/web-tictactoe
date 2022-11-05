# syntax=docker/dockerfile:1

# Build Angular
FROM node:alpine AS tictactoe-build
WORKDIR /app
COPY . .
RUN npm ci && npm run build

# Nginx serve
FROM nginx:alpine
COPY --from=tictactoe-build /app/dist/tictactoe /usr/share/nginx/html
EXPOSE 80
