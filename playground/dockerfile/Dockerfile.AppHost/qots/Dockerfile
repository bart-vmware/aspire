# Stage 1: Build the Go program
FROM golang:1.16 AS builder

WORKDIR /app

COPY . .

RUN go build qots.go

# Stage 2: Run the Go program
FROM golang:1.16

WORKDIR /app

COPY --from=builder /app/qots .

CMD ["./qots"]
