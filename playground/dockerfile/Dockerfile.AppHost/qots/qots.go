package main

import (
	"fmt"
	"math/rand"
	"time"
)

func main() {
	quotes := []string{
		"With great power comes great responsibility. - Spider-Man",
		"I'm Batman. - Batman",
		"I am Iron Man. - Iron Man",
		"Why so serious? - The Joker",
		"I'm always angry. - The Hulk",
	}

	rand.Seed(time.Now().UnixNano())

	for {
		quote := quotes[rand.Intn(len(quotes))]
		fmt.Println(quote)
		time.Sleep(time.Second)
	}
}
