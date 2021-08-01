using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrainScrew.Parser
{
    public class BrainScrewParser
    {
        const Int32 BufferSize = 128;
        char[] programCommands = { '>', '<', '+', '-', '.', ',', '[', ']' };

        private String brainScrewFileName;
        private List<Byte> memoryTape;
        private int pointer;
        private Stack<int> loopStack;

        public BrainScrewParser(String fileName)
        {
            brainScrewFileName = fileName;
            loopStack = new Stack<int>();
            memoryTape = new List<Byte>();
            memoryTape.Add(0);
            pointer = 0;
        }

        
        public void Compile()
        {

            using (var fileStream = File.OpenRead(brainScrewFileName))
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
            {
                String program;
                if((program = streamReader.ReadToEnd()) != null)
                {
                    int loopPairOpeningBrakets = 0, loopPairClosingBrakets = 0;
                    bool ignore = false;
                    for (int programCounter = 0; programCounter < program.Length;)
                    {
                        char command = program[programCounter];

                        if (!ignore || command == '[' || command == ']')
                        {
                            switch (command)
                            {
                                case '>':
                                    pointer++;
                                    if (pointer >= memoryTape.Count)
                                        memoryTape.Add(0);
                                    break;
                                case '<':
                                    pointer--;
                                    if (pointer < 0)
                                    {
                                        Console.WriteLine("Error: Pointer is out of bounds.");
                                        return;
                                    }
                                    break;
                                case '+':
                                    memoryTape[pointer]++;
                                    break;
                                case '-':
                                    memoryTape[pointer]--;
                                    break;
                                case ',':
                                    char input = Console.ReadKey().KeyChar;
                                    memoryTape[pointer] = (byte)input;
                                    break;
                                case '.':
                                    Console.Write((char)memoryTape[pointer]);
                                    break;
                                case '[':
                                    if (memoryTape[pointer] == 0 || ignore)
                                    {
                                        ignore = true;
                                        loopPairOpeningBrakets++;
                                    }else  
                                        loopStack.Push(programCounter);
                                    break;
                                case ']':
                                    if (ignore)
                                    {
                                        loopPairClosingBrakets++;
                                        if (loopPairOpeningBrakets == loopPairClosingBrakets)
                                        {
                                            loopPairOpeningBrakets = loopPairClosingBrakets = 0;
                                            ignore = false;
                                        }
                                        programCounter++;
                                    }
                                    else
                                        programCounter = loopStack.Pop();
                                    break;
                            }

                        }

                        if (command != ']')
                           programCounter++;
                    }
                }   
            }
        }
    }
}
