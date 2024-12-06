import re

# Define the path to the input file
INPUT_FILE_PATH = 'C:\\dev\\src\\nash\\AdventOfCode\\2024\\3\\input\\actual'

# Read the text contents of the file
with open(INPUT_FILE_PATH, 'r') as file:
    file_contents = file.read()

# Find all matches to the regular expression
matches = re.findall(r'(?:mul\((\d+),(\d+)\))|(do(?:n\'t)?\(\))', file_contents)

# Initialize the sum of products
sum_of_products = 0

do = True

# For each match, convert the captured strings to ints and multiply them together
for match in matches:
    if match[2] == "do()":
        do = True
    elif match[2] == "don't()":
        do = False
    elif do:
        num1 = int(match[0])
        num2 = int(match[1])
        product = num1 * num2
        sum_of_products += product

# # Write the sum of products to the console
print(sum_of_products)
