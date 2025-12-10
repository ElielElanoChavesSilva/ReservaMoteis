FROM node:18 AS build
WORKDIR /app
COPY package*.json ./
RUN npm install
COPY . .
RUN npm run build --prod

FROM nginx:stable
COPY --from=build /app/dist/nome-do-projeto /usr/share/nginx/html
EXPOSE 80
