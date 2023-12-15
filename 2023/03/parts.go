package main

import (
	"fmt"
	"os"
	"regexp"
	"strconv"
	"strings"
	"unicode"
)

// const inputFile = `C:\dev\src\nash\advent\20231203\input-sample.txt`

const inputFile = `C:\dev\src\nash\advent\20231203\input-actual.txt`

func isAPart(allLines []string, linenum int, index []int) bool {
	lowx := max(index[0]-1, 0)
	lowy := max(linenum-1, 0)
	highx := min(index[1], len(allLines[linenum])-1)
	highy := min(linenum+1, len(allLines)-1)

	for x := lowx; x <= highx; x++ {
		for y := lowy; y <= highy; y++ {
			if y == linenum && x >= index[0] && x < index[1] {
				continue
			}
			if allLines[y][x] != 46 {
				// fmt.Printf("%v %v %v %v %v %v %v %v\n", index, x, y, allLines[y][x], lowx, lowy, highx, highy)
				return true
			}
		}
	}
	return false
}

func sumAllPartNumbers(allLines []string) int {
	pattern := regexp.MustCompile(`(\d+)`)
	total := 0
	for lineNum, line := range allLines {
		indices := pattern.FindAllStringIndex(line, -1)
		nums := pattern.FindAllString(line, -1)
		for i, index := range indices {
			if isAPart(allLines, lineNum, index) {
				numVal, _ := strconv.Atoi(nums[i])
				total += numVal
			}
		}
	}
	return total
}

func sumAllGearRatios(allLines []string) int {
	total := 0
	for lineNum, line := range allLines {
		for pos, char := range line {
			if char == rune('*') {
				// fmt.Printf("Evaluating * at %v, %v\n", pos, lineNum)
				// 			// find adjacent parts
				// 			// 1 2 3
				// 			// 8 * 4
				// 			// 7 6 5
				p1 := getPartAtPosition(allLines[lineNum-1], pos-1)
				p2 := getPartAtPosition(allLines[lineNum-1], pos)
				p3 := getPartAtPosition(allLines[lineNum-1], pos+1)
				p4 := getPartAtPosition(line, pos+1)
				p5 := getPartAtPosition(allLines[lineNum+1], pos+1)
				p6 := getPartAtPosition(allLines[lineNum+1], pos)
				p7 := getPartAtPosition(allLines[lineNum+1], pos-1)
				p8 := getPartAtPosition(line, pos-1)

				gearRatio := 1
				partCount := 0
				if !processPart(p2, &gearRatio, &partCount) {
					processPart(p1, &gearRatio, &partCount)
					processPart(p3, &gearRatio, &partCount)
				}
				processPart(p4, &gearRatio, &partCount)
				if !processPart(p6, &gearRatio, &partCount) {
					processPart(p7, &gearRatio, &partCount)
					processPart(p5, &gearRatio, &partCount)
				}
				processPart(p8, &gearRatio, &partCount)

				if partCount == 2 {
					// fmt.Printf("Adding Gear Ratio of %v to the total\n", gearRatio)
					total += gearRatio
				}
			}
		}
	}
	return total
}

func processPart(part int, gearRatio, partCount *int) bool {
	if part == 0 {
		return false
	}
	*gearRatio *= part
	*partCount++
	return true
}

func getPartAtPosition(line string, x int) int {
	var start, end int
	for pos := x; pos >= 0 && unicode.IsDigit(rune(line[pos])); pos-- {
		start = pos
	}
	for pos := x; pos < len(line) && unicode.IsDigit(rune(line[pos])); pos++ {
		end = pos
	}
	if start == end && !unicode.IsDigit(rune(line[x])) {
		return 0
	}
	partStr := line[start : end+1]
	// fmt.Printf("Found %v at position %v of the line %v\n", partStr, x, line)
	partInt, _ := strconv.Atoi(partStr)
	return partInt
}

func main() {
	inputBytes, _ := os.ReadFile(inputFile)
	inputString := string(inputBytes)
	allLines := strings.Split(inputString, "\r\n")

	fmt.Printf("Answer #1: %v\n", sumAllPartNumbers(allLines))
	fmt.Printf("Answer #2: %v\n", sumAllGearRatios(allLines))
}
