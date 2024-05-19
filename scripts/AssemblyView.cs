using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public partial class AssemblyView : CodeEdit
{
    [ExportGroup("References")]
    [Export] private Label lineNums;

    [ExportGroup("Syntax colors")]
    [Export] private Color commentColor;
    [Export] private Color labelColor;
    [Export] private Color registerColor;
    [Export] private Color instructionColor;

    private List<int> codeLines;

    private List<string> instructions = new List<string> {"NOP", "HLT", "ADD", "SUB", "NOR", "AND", "XOR", "RSH", "LDI", "ADI", "JMP", "BRH", "CAL", "RET", "LOD", "STR", "CMP", "MOV", "LSH", "INC", "DEC", "NOT"};

    public bool initialized {get; private set;}

    public int programCounter;
    
    public override void _Ready()
    {
        CodeHighlighter highlighter = SyntaxHighlighter as CodeHighlighter;
        highlighter.AddColorRegion("//", "", commentColor);
        highlighter.AddColorRegion(".", " ", labelColor);
        for(int i = 0; i < 16; i++)
        {
            highlighter.AddKeywordColor("r" + i, registerColor);
        }
        foreach (string instruction in instructions)
        {
            highlighter.AddKeywordColor(instruction.ToLower(), instructionColor);
            highlighter.AddKeywordColor(instruction.ToUpper(), instructionColor);
        }

        codeLines = new List<int>();
    }

    public void LoadAssembly(string assembly)
    {
        string text = "";
        codeLines = new List<int>();
        int lineNum = 0;
        int pc = 0;
        using (StringReader sr = new StringReader(assembly)) {
            string line;
            while ((line = sr.ReadLine()) != null) {
                if (instructions.Any(line.ToUpper().Contains))
                {
                    codeLines.Add(lineNum);
                    text += padString("" + pc, 4, "0") + " | ";
                    pc++;
                } else text += "     | ";
                text += line + "\n";
                lineNum++;
            }
        }

        Text = text;
        initialized = true;
    }

    public void MoveCursor()
    {
        ClearExecutingLines();
        SetLineAsExecuting(codeLines[programCounter], true);
    }

    private string padString(string input, int length, string fill)
    {
        string output = "";
        for (int i = 0; i < length - input.Length; i++)
        {
            output += fill;
        }
        output += input;
        return output;
    }
}
