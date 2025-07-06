FROM node:22-alpine AS build

WORKDIR /usr/src/app

COPY package.json .
COPY package-lock.json .

RUN npm install

COPY . .
RUN npm run build --prod

FROM nginx:alpine

RUN rm -f /usr/share/nginx/html/*

COPY --from=build /usr/src/app/dist/Payment-Test/browser /usr/share/nginx/html

EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]