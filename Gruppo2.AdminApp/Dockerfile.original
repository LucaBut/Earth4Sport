FROM node as build
WORKDIR /app
COPY package*.json /app/
RUN npm install
COPY . /app
ARG configuration=production
RUN npm run build -- --outputPath=./dist/out --configuration $configuration
