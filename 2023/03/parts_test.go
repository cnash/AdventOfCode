package main

import "testing"

type getPartTestcase struct {
	a        string
	b        int
	expected int
}

func Test_getPartAtPosition(t *testing.T) {
	testcases := []getPartTestcase{
		{"...123...", 5, 123},
		{"123......", 2, 123},
		{"123......", 0, 123},
		{"......123", 7, 123},
		{"123...456", 5, 0},
		{".........", 0, 0},
		{"..5......", 2, 5},
	}

	for _, tc := range testcases {
		actual := getPartAtPosition(tc.a, tc.b)
		if actual != tc.expected {
			t.Errorf("getPartAtPosition(%v, %v) expected %v but got %v", tc.a, tc.b, tc.expected, actual)
		}
	}
}
