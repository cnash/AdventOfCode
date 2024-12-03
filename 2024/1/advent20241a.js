const inputFile = "G:\\My Drive\\code\\adventofcode\\2024\\1a\\input\\actual"

const fs = require('node:fs');

fs.readFile(inputFile, 'utf8', (err, data) => {
  if (err) {
    console.error(err);
    return;
  }
  let lines = data.split('\r\n');
  let col1=[];
  let col2=[];
  for (var i in lines)
  {
    var vals = lines[i].split("   ");
    col1.push(vals[0]);
    col2.push(vals[1]);
  }
  col1.sort();
  col2.sort();
  let total = 0;
  do
  {
    let val1 = col1.pop();
    if (val1 === undefined)
    {
        break;
    }
    let val2 = col2.pop();
    total += Math.abs(val1-val2);
} while (true);

  console.log(total);
});
