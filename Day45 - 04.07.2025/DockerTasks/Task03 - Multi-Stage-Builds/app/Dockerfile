FROM node:22-alpine AS build

WORKDIR /app

COPY package.json .

COPY package-lock.json .

RUN npm install

COPY . . 

RUN npm run build

# Server

FROM nginx:alpine

RUN rm -rf /usr/share/nginx/html/*

COPY --from=build /app/build  /usr/share/nginx/html

EXPOSE 80

CMD ["nginx", "-g","daemon off;"]