# syntax=docker/dockerfile:1

# Build Angular
FROM node:alpine AS tictactoe-build
WORKDIR /app
COPY . .
RUN npm ci && npm run build

# Nginx serve
FROM nginx:alpine
COPY --from=tictactoe-build /app/dist/tictactoe /usr/share/nginx/html
RUN rm /etc/nginx/conf.d/default.conf
COPY nginx.conf /etc/nginx/conf.d
EXPOSE 8080
