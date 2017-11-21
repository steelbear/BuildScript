namespace BuildScript.Parse
{
    enum TokenType
    {
        Unknown,
        EOF,
        EOL,

        Identifier,        // identifier
        Integer,           // 123, 0x123
        String,            // 'string'
        InterpolatedString,// "#{interpolated} string"

        // Keywords
        Break,             // break
        Case,              // case
        Default,           // default
        Else,              // else
        ElseIf,            // elseif
        False,             // false
        For,               // for
        Global,            // global
        If,                // if
        In,                // in
        Match,             // match
        Not,               // not
        Project,           // project
        Raise,             // raise
        Repeat,            // repeat
        Return,            // return
        Target,            // target
        Task,              // task
        True,              // true
        Undefined,         // undefined
        Until,             // until
        Var,               // var
        While,             // while

        // Punctuators
        LeftParen,         // (
        RightParen,        // )
        LeftBrace,         // {
        RightBrace,        // }
        LeftSquare,        // [
        RightSquare,       // ]
        Arrow,             // ->
        Assign,            // =
        Append,            // +=
        ConditionalAssign, // :=
        Plus,              // +
        Minus,             // -
        Multiply,          // *
        Divide,            // /
        Remainder,         // %
        Less,              // <
        LessOrEqual,       // <=
        Grater,            // >
        GraterOrEqual,     // >=
        Equal,             // ==
        NotEqual,          // !=
        LogicalAND,        // &&
        LogicalOR,         // ||
        Dot,               // .
        Comma,             // ,
        Colon,             // :
        ConditionMarker    // ?
    }
}
