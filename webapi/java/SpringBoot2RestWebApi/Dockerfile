FROM openjdk:8 as base
WORKDIR /app
EXPOSE 80

FROM openjdk:8 as build
WORKDIR /src
COPY . .
RUN ./gradlew build

FROM base as finall
COPY --from=build /src/build/libs .
ENTRYPOINT ["java","-Djava.security.egd=file:/dev/./urandom", "-jar","/app/SpringBoot2RestWebApi-1.0-SNAPSHOT.jar"]