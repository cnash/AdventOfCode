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
    col1.push(Number(vals[0]));
    col2.push(Number(vals[1]));
  }

  const similarity = col1.reduce(
    (acc, cur) => {
      let count = col2.filter((val) => val === cur).length;
      return acc + (cur * count);
    },
    0
  );

  console.log(similarity);
});
