Console.Write("Enter the name of the new PIL file (NOT including extension): ");
string pilfilename = Console.ReadLine();

// get numebr of pim files
int pimcount = 0;
bool valid = false;
string input;
while (!valid)
{
    Console.Write("Enter the number of PIM files to combine: ");
    input = Console.ReadLine();

    try
    {
        pimcount = int.Parse(input);
        valid = true;
        if (pimcount <= 0)
        {
            valid = false;
            Console.WriteLine("Invalid Input");
        }
    }
    catch (FormatException)
    {
        valid = false;
        Console.WriteLine("Invalid Input");
    }
}

// get pim file names
valid = false;
string[] pimnames = new string[pimcount];
for (int i = 0; i < pimcount; i++)
{
    while (!valid)
    {
        Console.Write("Enter the names of PIM files to combine: ");
        input = Console.ReadLine();
        if (input.Length <= 4)
        {
            Console.WriteLine("Invalid Input");
        }
        else if (!File.Exists(input))
        {
            Console.WriteLine("Invalid Input");
        }
        else
        {
            valid = true;
            pimnames[i] = input;
        }
    }
    valid = false;
}

// get size of byte array
int pilsize = 0x10; // pil header size
byte [][] pimfiles = new byte[pimcount][];
for (int i = 0; i < pimcount; i++)
{
    pimfiles[i] = File.ReadAllBytes(pimnames[i]);
    pilsize += pimfiles[i].Length + 0x10; // extra 16 bytes per pim file
}

// combine pim files
int byteend = 0;
byte[] pildata = new byte[pilsize];
pildata[0] = (byte)pimcount;
byteend += 0x10;
for (int i = 0; i < pimcount; i++)
{
    Console.WriteLine(i.ToString());
    // get first 8 bytes of pim header
    for (int j = 0; j < 0x08; j++)
    {
        pildata[byteend++] = pimfiles[i][j];
    }
    // get file name excluding extension
    for (int j = 0; j < pimnames[i].Length - 4; j++)
    {
        pildata[byteend + j] = (byte)pimnames[i][j];
    }
    byteend += 0x18;
    // write the remaing data
    for (int j = 0x10; j < pimfiles[i].Length; j++)
    {
        pildata[byteend++] = pimfiles[i][j];
    }
    File.WriteAllBytes(pilfilename + ".PIL", pildata);
}
File.WriteAllBytes(pilfilename + ".PIL", pildata);

if (File.Exists(pilfilename + ".PIL"))
{
    Console.WriteLine("Successfully created " + pilfilename);
}
else
{
    Console.WriteLine("Error: PIL File failed to create");
}