namespace Minesweeper.Properties
{
    class ConstructorField : BaseField
    {

        public ConstructorField(int width, int height, int mineCount) : base(width, height, mineCount) { }

        public Cell this[int i, int j]
        {
            get
            {
                if (i >= 0 && i < height && j >= 0 && j < width)
                    return field[i, j];
                return null;
            }
        }

    }
}
