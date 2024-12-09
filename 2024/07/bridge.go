package main

import (
	"fmt"
	"os"
	"strconv"
	"strings"
)

const inputFilePathSample = `C:\dev\src\nash\AdventOfCode\2024\07\input-sample`
const inputFilePathActual = `C:\dev\src\nash\AdventOfCode\2024\07\input-actual`
const expectedSampleResult = 3749
const expectedSampleResult2 = 11387
const DEBUG = false

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

func Problem1(input string) int {
	total := 0

	allLines := strings.Split(input, "\r\n")
	for _, line := range allLines {
		arr1 := strings.Split(line, ":")
		testValue, _ := strconv.Atoi(strings.TrimSpace(arr1[0]))
		arr2 := strings.Split(strings.TrimSpace(arr1[1]), " ")
		var calVals []int
		for _, valStr := range arr2 {
			val, _ := strconv.Atoi(valStr)
			calVals = append(calVals, val)
		}
		flag, msg := isValidCalibration(testValue, calVals)

		if flag {
			total += testValue
			DebugLog(fmt.Sprintf("%s = %d\n", msg, testValue))
		}
	}

	return total
}

func isValidCalibration(testValue int, calVals []int) (bool, string) {

	if len(calVals) == 1 {
		return (calVals[0] == testValue), fmt.Sprintf("%d", testValue)
	}

	lenCalValsMinus1 := len(calVals) - 1
	flag, msg := isValidCalibration(testValue-calVals[lenCalValsMinus1], calVals[:lenCalValsMinus1])
	if flag {
		return true, fmt.Sprintf("(%s + %d)", msg, calVals[lenCalValsMinus1])
	}

	if testValue%calVals[lenCalValsMinus1] == 0 {
		flag, msg = isValidCalibration(testValue/calVals[lenCalValsMinus1], calVals[:lenCalValsMinus1])
		if flag {
			return true, fmt.Sprintf("(%s x %d)", msg, calVals[lenCalValsMinus1])
		}
	}

	return false, ""
}

func Problem2(input string) int {
	total := 0

	allLines := strings.Split(input, "\r\n")
	for _, line := range allLines {
		arr1 := strings.Split(line, ":")
		testValue, _ := strconv.Atoi(strings.TrimSpace(arr1[0]))
		arr2 := strings.Split(strings.TrimSpace(arr1[1]), " ")
		var calVals []int
		for _, valStr := range arr2 {
			val, _ := strconv.Atoi(valStr)
			calVals = append(calVals, val)
		}
		flag := isValidCalibration3(testValue, calVals)

		if flag {
			total += testValue
		}
	}

	return total
}

func isValidCalibration2(testValue int, calVals []int) (bool, string) {

	if len(calVals) == 1 {
		return (calVals[0] == testValue), fmt.Sprintf("%d", testValue)
	}

	lenCalValsMinus1 := len(calVals) - 1
	flag, msg := isValidCalibration2(testValue-calVals[lenCalValsMinus1], calVals[:lenCalValsMinus1])
	if flag {
		return true, fmt.Sprintf("(%s + %d)", msg, calVals[lenCalValsMinus1])
	}

	if testValue%calVals[lenCalValsMinus1] == 0 {
		flag, msg = isValidCalibration2(testValue/calVals[lenCalValsMinus1], calVals[:lenCalValsMinus1])
		if flag {
			return true, fmt.Sprintf("(%s x %d)", msg, calVals[lenCalValsMinus1])
		}
	}

	newVal, _ := strconv.Atoi(fmt.Sprintf("%d%d", calVals[len(calVals)-2], calVals[lenCalValsMinus1]))
	newCalVals := make([]int, len(calVals)-1)
	copy(newCalVals, calVals[:len(calVals)-2])
	newCalVals[len(calVals)-2] = newVal
	flag, msg = isValidCalibration2(testValue, newCalVals)
	if flag {
		return true, fmt.Sprintf("(%d || %d)", calVals[len(calVals)-2], calVals[lenCalValsMinus1])
	}
	DebugLog(fmt.Sprintf("notValid %d, %s\n", testValue, arrayToString(calVals)))
	return false, ""
}

func DebugLog(msg string) {
	if DEBUG {
		fmt.Print(msg)
	}
}

func arrayToString(arr []int) string {
	var strArr []string
	for _, num := range arr {
		strArr = append(strArr, fmt.Sprintf("%d", num))
	}

	return fmt.Sprintf("[%s]", strings.Join(strArr, ", "))
}

func isValidCalibration3(testValue int, calVals []int) bool {
	DebugLog(fmt.Sprintf("isValidCalibration3(%d, %s)\n", testValue, arrayToString(calVals)))
	if len(calVals) == 1 {
		return (calVals[0] == testValue)
	}
	val1 := calVals[0] + calVals[1]
	val2 := calVals[0] * calVals[1]
	val3, _ := strconv.Atoi(fmt.Sprintf("%d%d", calVals[0], calVals[1]))

	if isValidCalibration3(testValue, push(val1, calVals[2:])) {
		return true
	}

	if isValidCalibration3(testValue, push(val2, calVals[2:])) {
		return true
	}

	if isValidCalibration3(testValue, push(val3, calVals[2:])) {
		return true
	}
	return false
}

func push(val int, array []int) []int {
	newArray := make([]int, len(array)+1)
	newArray[0] = val
	for i := 0; i < len(array); i++ {
		newArray[i+1] = array[i]
	}
	return newArray
}
