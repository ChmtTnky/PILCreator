Console.Write("Enter the name of the new PIL file (NOT including extension): ");
string pilfilename = Console.ReadLine();

// get numebr of pim files
int pim_count = 0;
bool valid = false;
string input;
while (!valid)
{
    Console.Write("Enter the number of PIM files to combine: ");
    input = Console.ReadLine();

    try
    {
        pim_count = int.Parse(input);
        valid = true;
        if (pim_count <= 0)
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
string[] pimnames = new string[pim_count];
for (int i = 0; i < pim_count; i++)
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
int pil_size = 0x10; // pil header size
byte [][] pimfiles = new byte[pim_count][];
for (int i = 0; i < pim_count; i++)
{
    pimfiles[i] = File.ReadAllBytes(pimnames[i]);
    pil_size += pimfiles[i].Length + 0x10; // extra 16 bytes per pim file
}

// combine pim files
int byte_end = 0;
byte[] pildata = new byte[pil_size];
pildata[0] = (byte)pim_count;
byte_end += 0x10;
for (int i = 0; i < pim_count; i++)
{
    Console.WriteLine(i.ToString());
    // get first 8 bytes of pim header
    for (int j = 0; j < 0x08; j++)
    {
        pildata[byte_end++] = pimfiles[i][j];
    }
    // get file name excluding extension
    for (int j = 0; j < pimnames[i].Length - 4; j++)
    {
        pildata[byte_end + j] = (byte)pimnames[i][j];
    }
    byte_end += 0x18;
    // write the remaing data
    for (int j = 0x10; j < pimfiles[i].Length; j++)
    {
        pildata[byte_end++] = pimfiles[i][j];
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