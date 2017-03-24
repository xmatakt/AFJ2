using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LA.Enums;

namespace LA.Classes
{
    //  trieda reprezentujuca konecny deterministicky automat
    class DKA
    {
        private readonly Dictionary<int, string> codeToWordMapping;
        private string[] inputFile;
        private readonly State initialState;
        private bool isCreatedSuccessully = false;

        public DKA(string path)
        {
            codeToWordMapping = new Dictionary<int, string>();
            initialState = new State("q0", true);

            var creatingResult = CreateDKA(path);

            switch (creatingResult)
            {
                case ReturnCodesEnum.SourceFileDoesntExist:
                    Console.WriteLine("Error while reading input file:\nFile {0} doesn't exist!", path);
                    break;
                case ReturnCodesEnum.InputFileInWrongFormat:
                    Console.WriteLine("The file with the language was not in the correct format!");
                    break;
                case ReturnCodesEnum.DKACreationSuccess:
                    isCreatedSuccessully = true;
                    break;
            }
        }

        public void AnalyzeText(string path)
        {
            var analyzeResult = Analyze(path);
            switch (analyzeResult)
            {
                case ReturnCodesEnum.LexicalError:
                    Console.WriteLine("\nPress any key to exit...");
                    break;
                case ReturnCodesEnum.DKACreationFailed:
                    Console.WriteLine("Instance of DKA wasn't created succesfully!");
                    break;
                case ReturnCodesEnum.SourceFileDoesntExist:
                    Console.WriteLine("Error while reading input file:\nFile {0} doesn't exist!", path);
                    break;
                case ReturnCodesEnum.ExitSuccess:
                    Console.WriteLine("\nPress any key to exit...");
                    break;
            }
        }

        private ReturnCodesEnum Analyze(string path)
        {
            if(!isCreatedSuccessully)
                return ReturnCodesEnum.DKACreationFailed;
            if (!File.Exists(path))
                return ReturnCodesEnum.SourceFileDoesntExist;


            Console.WriteLine("{0,-10}\t{1,-10}", "Word:", "Code:");
            inputFile = File.ReadAllLines(path);
            for (var i = 0; i < inputFile.Length; i++)
            {
                var activeState = initialState;
                foreach (var word in  inputFile[i].Split(new char[] {' ', '\t'}))
                {
                    var result = ProcessWord(word);
                    if (result == -1)
                    {
                        Console.WriteLine("LEXICAL ERROR on line number {0}!", i + 1);
                        return ReturnCodesEnum.LexicalError;
                    }
                    if (result != -2)
                        Console.WriteLine("{0,-10}\t{1,-10}", codeToWordMapping[result], result);
                }
            }

            return ReturnCodesEnum.ExitSuccess;
        }

        private int ProcessWord(string word)
        {
            var indexToStateMapping = new Dictionary<int, State>();
            var activeState = initialState;

            for (var i = 0; i < word.Length; i++)
            {
                activeState = activeState.GetState(word[i]);

                //  ak pre i-te pismeno slova neexistuje prechod
                if (activeState == null)
                {
                    //  pokusim sa najst posledny akceptovatelny stav
                    var lastAcceptableState = indexToStateMapping.LastOrDefault(x => x.Value.GetStateAcceptability());
                    if (lastAcceptableState.Value == null)
                        return -1;

                    i = lastAcceptableState.Key;
                    activeState = initialState;
                    Console.WriteLine("{0,-10}\t{1,-10}", codeToWordMapping[lastAcceptableState.Value.GetStateCode()],
                        lastAcceptableState.Value.GetStateCode());
                    indexToStateMapping.Clear();
                }
                else
                {
                    indexToStateMapping.Add(i, activeState);
                }

                //  ak sa spracovalo cele slovo, zistim, ci je aktualny stav akceptovatelny
                if (i == word.Length - 1)
                {
                    return activeState.GetStateCode();
                }
            }

            return -2;
        }

        private ReturnCodesEnum CreateDKA(string path)
        {
            if (!File.Exists(path))
                return ReturnCodesEnum.SourceFileDoesntExist;

            var parsingResult = ParseInputFile(path);
            if(parsingResult != ReturnCodesEnum.ExitSuccess)
                return parsingResult;

            var activeState = initialState;

            foreach (var keyValuePair in codeToWordMapping)
            {
                var word = keyValuePair.Value;
                var wordCode = keyValuePair.Key;
                for (var i = 0; i < word.Length; i++)
                {
                    var isAcceptabelState = (i == word.Length - 1);
                    var code = isAcceptabelState ? wordCode : -1;

                    var existingState = activeState.GetState(word[i]);
                    if (existingState == null)
                    {
                        var newState = new State("q_" + word.Substring(0, i + 1), false, isAcceptabelState, code);
                        activeState.AddTrasition(word[i], newState);
                        activeState = isAcceptabelState ? initialState : newState;
                    }
                    else
                    {
                        if(isAcceptabelState)
                            existingState.ModifyState(code);
                        activeState =  isAcceptabelState ? initialState : existingState;
                    }
                }
            }

            return ReturnCodesEnum.DKACreationSuccess;
        }

        private ReturnCodesEnum ParseInputFile(string path)
        {
            foreach (var line in File.ReadAllLines(path))
            {
                var splittedLine = line.Split(' ');
                try
                {
                    var word = splittedLine[0];
                    var code = int.Parse(splittedLine[1]);
                    if (code < 0)
                        throw new Exception("Negative codes are not supported!");
                    codeToWordMapping.Add(code, word);
                }
                catch (ArgumentException e)
                {
                    Console.WriteLine("Two words can't have the same code!");
                    return ReturnCodesEnum.InputFileInWrongFormat;
                }
                catch (IndexOutOfRangeException e)
                {
                    return ReturnCodesEnum.InputFileInWrongFormat;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return ReturnCodesEnum.InputFileInWrongFormat;
                }
            }

            return ReturnCodesEnum.ExitSuccess;
        }
    }
}
