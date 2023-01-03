FROM node:18-alpine AS build
COPY package.json package-lock.json ./
RUN npm ci && mkdir /app && mv ./node_modules ./app
WORKDIR /app
COPY . .
RUN npm run ng build -- --configuration=production --output-path=dist

FROM nginx:alpine
RUN rm /usr/share/nginx/html/index.html
COPY --from=build /app/dist /usr/share/nginx/html
RUN rm /etc/nginx/conf.d/default.conf
COPY nginx.conf /etc/nginx/conf.d
EXPOSE 8080
