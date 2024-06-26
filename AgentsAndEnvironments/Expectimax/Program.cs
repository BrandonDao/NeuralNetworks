﻿namespace Expectimax
{
    public struct Move<TState>(TState result, double probability)
        where TState : IState<TState>
    {
        public TState Result = result;
        public double Probability = probability;
    }

    public interface IState<TState>
        where TState : IState<TState>
    {
        bool IsMaximizer { get; }
        bool IsTerminal { get; }
        bool IsDeterministic { get; }
        double Score { get; set; }
        Move<TState>[] GetSuccessors();
    }


    public class Expectimax<TState>
        where TState : IState<TState>
    {
        public TState Root { get; }

        public Expectimax(TState startState)
        {
            Root = startState;
        }

        public void PropagateScores()
        {
            PropogateScores(Root);

            static double PropogateScores(TState state)
            {
                if (state.IsTerminal)
                {
                    return state.Score;
                }

                Move<TState>[] moves = state.GetSuccessors();
                IEnumerable<double> scores = moves.Select((move) => move.Probability * PropogateScores(move.Result));

                if (state.IsDeterministic)
                {
                    state.Score = state.IsMaximizer ? scores.Max() : scores.Min();
                }
                else
                {
                    state.Score = scores.Sum();
                }

                return state.Score;
            }
        }
    }



    public class ExampleState(
            bool isTerminal,
            bool isDeterministic,
            bool isMax,
            Move<ExampleState>[] children,
            double score)
        : IState<ExampleState>
    {
        public bool IsMaximizer { get; } = isMax;
        public bool IsTerminal { get; } = isTerminal;
        public bool IsDeterministic { get; } = isDeterministic;
        public double Score { get; set; } = score;

        private readonly Move<ExampleState>[] children = children;

        public Move<ExampleState>[] GetSuccessors() => children;
    }

    #region old
    //public struct Board
    //{
    //    [Flags]
    //    public enum CellType : byte
    //    {
    //        Empty = 0,
    //        X = 0b01,
    //        O = 0b10,

    //        LeftMask = 0b11 << 4,
    //        MidMask = 0b11 << 2,
    //        RightMask = 0b11,

    //        XWinAcross = X << 4 | X << 2 | X,
    //        OWinAcross = O << 4 | O << 2 | O,

    //        Left_O = O,
    //        Left_X = X,
    //        Mid_O = O << 2,
    //        Mid_X = X << 2,
    //        Right_O = O << 4,
    //        Right_X = X << 4,
    //    }

    //    public CellType[] board;

    //    public Board()
    //    {
    //        board = new CellType[3];
    //    }

    //    public Board(Board other)
    //    {
    //        board = new CellType[3];

    //        for (int r = 0; r < 3; r++)
    //        {
    //            board[r] = other.board[r];
    //        }
    //    }

    //    public static bool operator ==(Board lhs, Board rhs)
    //    {
    //        for (int i = 0; i < 3; i++)
    //        {
    //            if (lhs.board[i] != rhs.board[i]) return false;
    //        }
    //        return true;
    //    }
    //    public static bool operator !=(Board lhs, Board rhs)
    //    {
    //        for (int i = 0; i < 3; i++)
    //        {
    //            if (lhs.board[i] != rhs.board[i]) return true;
    //        }
    //        return false;
    //    }
    //    public override readonly bool Equals(object? obj) => base.Equals(obj);
    //    public override readonly int GetHashCode() => base.GetHashCode();

    //    public readonly CellType TopLeft { get => board[0] & CellType.LeftMask; set => board[0] = (board[0] & ~CellType.LeftMask) | (CellType)((int)value << 4); }
    //    public readonly CellType TopMid { get => board[0] & CellType.MidMask; set => board[0] = (board[0] & ~CellType.MidMask) | (CellType)((int)value << 2); }
    //    public readonly CellType TopRight { get => board[0] & CellType.RightMask; set => board[0] = (board[0] & ~CellType.RightMask) | value; }
    //    public readonly CellType MidLeft { get => board[1] & CellType.LeftMask; set => board[1] = (board[1] & ~CellType.LeftMask) | (CellType)((int)value << 4); }
    //    public readonly CellType Mid { get => board[1] & CellType.MidMask; set => board[1] = (board[1] & ~CellType.MidMask) | (CellType)((int)value << 2); }
    //    public readonly CellType MidRight { get => board[1] & CellType.RightMask; set => board[1] = (board[1] & ~CellType.RightMask) | value; }
    //    public readonly CellType LowLeft { get => board[2] & CellType.LeftMask; set => board[2] = (board[2] & ~CellType.LeftMask) | (CellType)((int)value << 4); }
    //    public readonly CellType LowMid { get => board[2] & CellType.MidMask; set => board[2] = (board[2] & ~CellType.MidMask) | (CellType)((int)value << 2); }
    //    public readonly CellType LowRight { get => board[2] & CellType.RightMask; set => board[2] = (board[2] & ~CellType.RightMask) | value; }

    //}

    //enum TicTacToePlayers : byte
    //{
    //    X,
    //    O
    //}
    //public struct TicTacToeState : IState<TicTacToeState, TicTacToePlayers>
    //{
    //    public bool IsTerminal { get; }
    //    public Dictionary<TicTacToePlayers, int> PlayerToScore { get; }

    //    public readonly Board Board;

    //    private IState<TicTacToeState, TicTacToePlayers>.Move[]? children;

    //    public TicTacToeState()
    //    {

    //    }

    //    public IState<TicTacToeState, TicTacToePlayers>.Move[] GetSuccessors()
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public bool EvaluateBoard()
    //    {
    //        // empty board check
    //        if (Board.board[0] == CellType.Empty && Board.board[1] == CellType.Empty && Board.board[2] == CellType.Empty)
    //        {
    //            PlayerToScore[] = 0;
    //            return false;
    //        }

    //        // row checks
    //        if (Board.board[0] == CellType.XWinAcross || Board.board[1] == CellType.XWinAcross || Board.board[2] == CellType.XWinAcross)
    //        {
    //            Score = 1;
    //            return true;
    //        }
    //        if (Board.board[0] == CellType.OWinAcross || Board.board[1] == CellType.OWinAcross || Board.board[2] == CellType.OWinAcross)
    //        {
    //            Score = -1;
    //            return true;
    //        }

    //        // column checks
    //        int count;
    //        int otherCount;
    //        for (int c = 0; c < 3; c++)
    //        {
    //            count = 0;
    //            otherCount = 0;
    //            CellType mask = (CellType)(0b11 << (c << 1));

    //            for (int r = 0; r < 3; r++)
    //            {
    //                if (((int)(Board.board[r] & mask) >> (c << 1)) == (int)CellType.X)
    //                {
    //                    count++;
    //                }
    //                else if (((int)(Board.board[r] & mask) >> (c << 1)) == (int)CellType.O)
    //                {
    //                    otherCount++;
    //                }
    //            }

    //            if (count == 3)
    //            {
    //                Score = 1;
    //                return true;
    //            }
    //            if (otherCount == 3)
    //            {
    //                Score = -1;
    //                return true;
    //            }
    //        }

    //        // diagonal top/left -> bottom/right
    //        count = 0;
    //        for (int rc = 0; rc < 3; rc++)
    //        {
    //            CellType mask = (CellType)(0b11 << (rc << 1));
    //            count += (byte)(Board.board[rc] & mask);
    //        }
    //        if (count == 0b0001_0101)
    //        {
    //            Score = 1;
    //            return;
    //        }
    //        if (count == 0b0010_1010)
    //        {
    //            Score = -1;
    //            return;
    //        }

    //        // diagonal top/right -> bottom/left
    //        count = 0;
    //        for (int rc = 0; rc < 3; rc++)
    //        {
    //            CellType mask = (CellType)(0b11 << (rc << 1));
    //            count += (byte)(Board.board[2 - rc] & mask);
    //        }
    //        if (count == 0b0001_0101)
    //        {
    //            Score = 1;
    //            return;
    //        }
    //        if (count == 0b0010_1010)
    //        {
    //            Score = -1;
    //            return;
    //        }

    //        // draw check
    //        count = 0;
    //        for (int c = 0; c < 3; c++)
    //        {
    //            CellType mask = (CellType)(0b11 << (c << 1));

    //            for (int r = 0; r < 3; r++)
    //            {
    //                if ((Board.board[r] & mask) != CellType.Empty)
    //                {
    //                    count++;
    //                }
    //            }
    //        }
    //        if (count == 9)
    //        {
    //            Score = 0;
    //            return;
    //        }

    //        // assume game is not over
    //        IsTerminal = false;
    //        Score = 0;
    //    }
    //}
    #endregion

    public class Program
    {
        private static void Main()
        {
            ExampleState root = new(
                isTerminal: false,
                isDeterministic: true,
                isMax: true,
                children: [
                    new(result: new(isTerminal: false,
                                    isDeterministic: false,
                                    isMax: false,
                                    children: [
                                        new(result: new(isTerminal: true,
                                                        isDeterministic: true,
                                                        isMax: true,
                                                        children: [],
                                                        score: -3),
                                            probability: 0.33),
                                        new(result: new(isTerminal: true,
                                                        isDeterministic: true,
                                                        isMax: true,
                                                        children: [],
                                                        score: 6),
                                            probability: 0.67)],
                                    score: 0),
                        probability: 1),
                    new(result: new(isTerminal: false,
                                    isDeterministic: true,
                                    isMax: false,
                                    children: [
                                        new(result: new(isTerminal: true,
                                                        isDeterministic: true,
                                                        isMax: true,
                                                        children: [],
                                                        score: 7),
                                            probability: 1),
                                        new(result: new(isTerminal: true,
                                                        isDeterministic: true,
                                                        isMax: true,
                                                        children: [],
                                                        score: 0),
                                            probability: 1)],
                                    score: 0),
                        probability: 1),
                    new(result: new(isTerminal: false,
                                    isDeterministic: false,
                                    isMax: false,
                                    children: [
                                        new(result: new(isTerminal: true,
                                                        isDeterministic: true,
                                                        isMax: true,
                                                        children: [],
                                                        score: 3),
                                            probability: 0.33),
                                        new(result: new(isTerminal: true,
                                                        isDeterministic: true,
                                                        isMax: true,
                                                        children: [],
                                                        score: 0),
                                            probability: 0.67)],
                                    score: 0),
                        probability: 1)],
                score: 0);


            Expectimax<ExampleState> tree = new(root);
            tree.PropagateScores();

            ;
        }
    }
}