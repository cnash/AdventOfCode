package main

import (
	"fmt"
	"os"
	"slices"
	"strings"
)

const inputFilePathSample = `C:\dev\src\nash\AdventOfCode\2024\06\input-sample`
const inputFilePathActual = `C:\dev\src\nash\AdventOfCode\2024\06\input-actual`
const expectedSampleResult = 41
const expectedSampleResult2 = 6

func main() {
	inputBytes, _ := os.ReadFile(inputFilePathSample)
	inputString := string(inputBytes)
	fmt.Printf("(P1 Sample) Expected: %v, Actual: %v\n", expectedSampleResult, Problem1(inputString))
	inputBytes, _ = os.ReadFile(inputFilePathActual)
	inputString = string(inputBytes)
	fmt.Printf("(P1 Actual) %v\n", Problem1(inputString))

	inputBytes, _ = os.ReadFile(inputFilePathSample)
	inputString = string(inputBytes)
	fmt.Printf("(P2 Sample) Expected: %v, Actual: %v\n", expectedSampleResult2, Problem2(inputString))
	inputBytes, _ = os.ReadFile(inputFilePathActual)
	inputString = string(inputBytes)
	fmt.Printf("(P2 Actual) %v\n", Problem2(inputString))

}

type coords struct {
	X, Y int
}

//Facings
//  0
// 3 1
//  2

func parseMap(input string) ([][]rune, coords, coords) {
	guardPos := coords{-1, -1}
	allLines := strings.Split(input, "\r\n")
	var floorMap [][]rune
	for y, line := range allLines {
		gpos := strings.IndexRune(line, '^')
		if gpos != -1 {
			guardPos.X = gpos
			guardPos.Y = y
		}

		chars := []rune(line)
		floorMap = append(floorMap, chars)
	}
	maxX := len(floorMap[0]) - 1
	maxY := len(floorMap) - 1
	return floorMap, coords{guardPos.X, guardPos.Y}, coords{maxX, maxY}
}

func showMap(floorMap [][]rune) {
	for _, line := range floorMap {
		for _, spot := range line {
			fmt.Printf("%c", spot)
		}
		fmt.Println()
	}
}

func nextPos(curPos coords, facing int) coords {
	nextX := curPos.X
	nextY := curPos.Y
	switch facing {
	case 0:
		nextY--
	case 1:
		nextX++
	case 2:
		nextY++
	case 3:
		nextX--
	}
	return coords{nextX, nextY}
}

func Problem1(input string) int {
	floorMap, guardPos, maxPos := parseMap(input)
	guardFacing := 0
	path := make(map[coords][]int)

	for {
		savePath(path, guardPos, guardFacing)
		next := nextPos(guardPos, guardFacing)
		if next.X > maxPos.X || next.X < 0 || next.Y > maxPos.Y || next.Y < 0 {
			break
		}
		if floorMap[next.Y][next.X] == '#' {
			guardFacing = (guardFacing + 1) % 4
		} else {
			guardPos.X = next.X
			guardPos.Y = next.Y
		}
	}
	return len(path)
}

func savePath(path map[coords][]int, pos coords, facing int) {
	val, ok := path[pos]
	if !ok {
		val = make([]int, 0)
	}
	if !slices.Contains(val, facing) {
		val = append(val, facing)
		path[pos] = val
	}
}

func doneBefore(path map[coords][]int, pos coords, facing int) bool {
	val, ok := path[pos]
	if !ok {
		return false
	}
	return slices.Contains(val, facing)
}

func willLoop(input string) bool {
	floorMap, guardPos, maxPos := parseMap(input)
	guardFacing := 0

	path := make(map[coords][]int)
	for {
		savePath(path, guardPos, guardFacing)

		next := nextPos(guardPos, guardFacing)
		if doneBefore(path, next, guardFacing) {
			return true
		}
		if next.X > maxPos.X || next.X < 0 || next.Y > maxPos.Y || next.Y < 0 {
			break
		}
		if floorMap[next.Y][next.X] == '#' {
			guardFacing = (guardFacing + 1) % 4
		} else {
			guardPos = next
		}
	}

	return false
}
func replaceRuneAtIndex(s string, newRune rune, index int) string {
	runes := []rune(s)
	runes[index] = newRune
	return string(runes)
}

func Problem2(input string) int {
	total := 0
	for i, r := range input {
		if r == '.' {
			newString := replaceRuneAtIndex(input, '#', i)
			if willLoop(newString) {
				total++
			}
		}
	}
	return total
}
