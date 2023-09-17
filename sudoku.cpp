// sudoku.cpp - simple console game
#include <iostream>
#include <random>

#define PRINT_INFO(variable_name, result) std::cout << variable_name << " : " << result << std::endl;

enum Color {
      BlackFg = 90,
      BlackBg = 40,
      RedFg = 31,
      RedBg = 41,
      GreenFg = 32,
      GreenBg = 42,
      YellowFg = 33,
      YellowBg = 43,
      MagentaFg = 35,
      MagentaBg = 45,
      CyanFg = 36,
      CyanBg = 46,
      Default = 0
};

void game();

int **mtrxAllocate(int dimension);
void freeMtrxs(int ***defaultDigits, int ***currentDigits, int ***solvedMtrx, int dimension);
void freeMtrx(int ***mtrxPtr, int len);

Color getFgColor(int **digits, int **defaultDigits, int dimension, int row, int col, int playerPosX,
                 int playerPosY);

void initDigits(int ***defaultDigitsPtr, int ***solvedDigitsPtr, int ***currentDigitsPtr, int dimension);
void fillMtrxOfRandomDigitsFully(int ***mtrxPtr, int dimension);
int getRandomUnusedDigit(int ***mtrxPtr, int dimension, int verticalIndex, int horizontalIndex);
bool hasDigitInHorizontalLine(int **mtrx, int dimension, int verticalIndex, int horizontalIndex, int digit);
bool hasDigitInVerticalLine(int **mtrx, int dimension, int verticalIndex, int horizontalIndex, int digit);
bool hasDigitInCurrentSection(int **mtrx, int digit, int horizontalIndex, int verticalIndex);

void hideAnyRandomDigits(int ***digitsPtr);
void hideDigitsInThisSection(int ***digitPtr, int amountOfHiddenDigits, int startRow, int startCol);

void copyMtrx(int **from, int ***to, int dimension);

void wipeOfLine(int ***digitsPtr, int dimension, int indexOfLine);

void drawField(int **digits, int **defaultDigits, int digitsDimension, int playerPosX, int playerPosY);

void doActionWithPlayerInput(char *playerInput, int *playerPosX, int *playerPosY, int ***currentDigits,
                             int ***defaultDigits, int ***solvedMtrx, int digitsDimension,
                             int *digitsShownCount, bool *isMenuShown);

bool isDefaultDigit(int **currentDigits, int **defaultDigits, int row, int col);
bool isEnteredDigitCorrect(int **digits, int dimension, int row, int col);
int getCountOfShownNumbers(int **defaultDigits, int dimension);

void clear();
void showHint(int ***solvedMtrx, int ***defaultDigits, int dimension);
void showMenu(int digitsShownCount);
void showRules();
void showEndOfGame(int filledSellsCount);
void refreshField(int ***currentDigits, int **defaultDigits, int dimension, int *playerPosX, int *playerPosY,
                  int *digitsShownCount);
void refreshCurrentData(int **defaultDigits, int dimension, int *playerPosX, int *playerPosY,
                        int *digitsShownCount);
void recreateDigits(int ***currentDigits, int ***defaultDigits, int ***solvedMtrx, int dimension,
                    int *playerPosX, int *playerPosY, int *digitsShownCount);
void clearSell(int ***currentDigits, int ***defaultDigits, int row, int col, int dimension,
               int *digitsShownCount);
void changeSell(int ***currentDigits, int ***defaultDigits, int row, int col, int dimension, char playerInput,
                int *digitsShownCount);
template <typename T>
void setColor(int fgColor, int bgColor, T text);

// Tasks:
// 1. If digit was correct and than we change the other value and this value
// became incorrect - we need to decrease an count of shown digits
// 2. Test the code;
// 3. Change arrays transmission from copying to pointers;

int main() {
      srand(time(NULL));

      game();

      return 0;
}
// checked
void game() {
      const int DIMENSION = 9;
      const int END_COUNT = 81;
      const int MIDDLE = 4;
      const char ESC = '\x1B';

      int **defaultDigits = mtrxAllocate(DIMENSION);
      int **currentDigits = mtrxAllocate(DIMENSION);
      int **solvedMtrx = mtrxAllocate(DIMENSION);
      initDigits(&defaultDigits, &solvedMtrx, &currentDigits, DIMENSION);

      int playerPosX = MIDDLE, playerPosY = MIDDLE;
      int digitsShownCount = getCountOfShownNumbers(defaultDigits, DIMENSION);
      int initialDigitsShownCount = digitsShownCount;

      char playerInput;

      bool isMenuShown = true;

      showRules();

      do {
            drawField(currentDigits, defaultDigits, DIMENSION, playerPosX, playerPosY);
            if (isMenuShown)
                  showMenu(digitsShownCount);
            else
                  setColor(Color::BlackFg, Color::Default, "\nm - show menu;\n");

            doActionWithPlayerInput(&playerInput, &playerPosX, &playerPosY, &currentDigits, &defaultDigits,
                                    &solvedMtrx, DIMENSION, &digitsShownCount, &isMenuShown);

            clear();
      } while (playerInput != ESC && digitsShownCount != END_COUNT);

      if (digitsShownCount == END_COUNT) showEndOfGame(END_COUNT - initialDigitsShownCount);

      freeMtrxs(&defaultDigits, &currentDigits, &solvedMtrx, DIMENSION);
}
// checked
int getCountOfShownNumbers(int **defaultDigits, int dimension) {
      int result = 0;

      for (int i = 0; i < dimension; i++) {
            for (int j = 0; j < dimension; j++) {
                  result = *(*(defaultDigits + i) + j) != 0 ? result + 1 : result;
            }
      }

      return result;
}
// checked
int **mtrxAllocate(int dimension) {
      int **ptr = new int *[dimension];

      for (int i = 0; i < dimension; i++) *(ptr + i) = new int[dimension];

      return ptr;
}
// checked
void freeMtrxs(int ***defaultDigits, int ***currentDigits, int ***solvedMtrx, int dimension) {
      freeMtrx(defaultDigits, dimension);
      freeMtrx(currentDigits, dimension);
      freeMtrx(solvedMtrx, dimension);
}
// checked
void freeMtrx(int ***mtrxPtr, int len) {
      for (int i = 0; i < len; i++) delete[] * (*mtrxPtr + i);

      delete[](*mtrxPtr);
}
// checked
void clear() { std::cout << "\x1B[2J\x1B[H"; }
// checked
void drawField(int **digits, int **defaultDigits, int digitsDimension, int playerPosX, int playerPosY) {
      using std::cout, std::endl;

      const int SIDE_BORDERS_COUNT = 4;
      const char VERTICAL_LINE = '|', HORIZONTAL_LINE = '-';
      const char SPACE_BAR = ' ';
      const char HIDDEN_CELL = '*';

      Color fgColor = Color::Default, bgColor = Color::Default;

      for (int i = 0, row = 0; i < digitsDimension + SIDE_BORDERS_COUNT; i++) {
            for (int j = 0, col = 0; j < digitsDimension + SIDE_BORDERS_COUNT; j++) {
                  cout << SPACE_BAR;

                  fgColor =
                      getFgColor(digits, defaultDigits, digitsDimension, row, col, playerPosX, playerPosY);

                  if (i % 4 == 0)
                        setColor(Color::Default, Color::Default, HORIZONTAL_LINE);
                  else if (j % 4 == 0)
                        setColor(Color::Default, Color::Default, VERTICAL_LINE);
                  else if (*(*(digits + row) + col) == 0)
                        setColor(fgColor, bgColor, HIDDEN_CELL);
                  else
                        setColor(fgColor, bgColor, *(*(digits + row) + col));

                  col = j % 4 != 0 ? col + 1 : col;
            }

            row = i % 4 != 0 && row + 1 != digitsDimension ? row + 1 : row;

            cout << endl;
      }
}
// checked
Color getFgColor(int **digits, int **defaultDigits, int dimension, int row, int col, int playerPosX,
                 int playerPosY) {
      Color result;

      const int HIDDEN_DIGIT = 0;

      bool isDefaultDigit = *(*(defaultDigits + row) + col) == *(*(digits + row) + col);
      bool isShownDigit = (*(*(digits + row) + col)) != HIDDEN_DIGIT;

      if (row == playerPosY && col == playerPosX)
            result = Color::CyanFg;
      else if (!isEnteredDigitCorrect(digits, dimension, row, col) && !isDefaultDigit && isShownDigit)
            result = Color::RedFg;
      else if (isDefaultDigit && isShownDigit)
            result = Color::YellowFg;
      else if (!isDefaultDigit)
            result = Color::GreenFg;
      else
            result = Color::BlackFg;

      return result;
}
// checked
bool isEnteredDigitCorrect(int **digits, int dimension, int row, int col) {
      return !((hasDigitInHorizontalLine(digits, dimension, row, col, *(*(digits + row) + col)) ||
                hasDigitInVerticalLine(digits, dimension, row, col, *(*(digits + row) + col))) ||
               hasDigitInCurrentSection(digits, *(*(digits + row) + col), col, row));
}
// checked
template <typename T>
void setColor(int fgColor, int bgColor, T text) {
      std::cout << "\033[" << bgColor << ";" << fgColor << "m" << text << "\033[" << Color::Default << "m";
}
// checked
void doActionWithPlayerInput(char *playerInput, int *playerPosX, int *playerPosY, int ***currentDigits,
                             int ***defaultDigits, int ***solvedMtrx, int digitsDimension,
                             int *digitsShownCount, bool *isMenuShown) {
      const int INDENT_TO_CAPITAL_LETTER = 32;

      std::cout << "\nInput: _\b";
      std::cin >> *playerInput;
      *playerInput =
          *playerInput >= 'A' && *playerInput <= 'Z' ? *playerInput + INDENT_TO_CAPITAL_LETTER : *playerInput;

      switch (*playerInput) {
            case 'w':
                  *playerPosY = (*playerPosY) - 1 != -1 ? (*playerPosY) - 1 : (*playerPosY);
                  break;
            case 's':
                  *playerPosY = (*playerPosY) + 1 != digitsDimension ? (*playerPosY) + 1 : (*playerPosY);
                  break;
            case 'a':
                  *playerPosX = (*playerPosX) - 1 != -1 ? (*playerPosX) - 1 : (*playerPosX);
                  break;
            case 'd':
                  *playerPosX = (*playerPosX) + 1 != digitsDimension ? (*playerPosX) + 1 : (*playerPosX);
                  break;
            case '0':
                  clearSell(currentDigits, defaultDigits, *playerPosY, *playerPosX, digitsDimension,
                            digitsShownCount);
                  break;
            case '1':
            case '2':
            case '3':
            case '4':
            case '5':
            case '6':
            case '7':
            case '8':
            case '9':
                  changeSell(currentDigits, defaultDigits, *playerPosY, *playerPosX, digitsDimension,
                             *playerInput, digitsShownCount);
                  break;
            case 'm':
                  *isMenuShown = true;
                  break;
            case 'c':
                  *isMenuShown = false;
                  break;
            case 'r':
                  refreshField(currentDigits, *defaultDigits, digitsDimension, playerPosX, playerPosY,
                               digitsShownCount);
                  break;
            case 'h':
                  showHint(solvedMtrx, defaultDigits, digitsDimension);
                  break;
            case 'n':
                  recreateDigits(currentDigits, defaultDigits, solvedMtrx, digitsDimension, playerPosX,
                                 playerPosY, digitsShownCount);
                  break;
      }
}
// checked
void changeSell(int ***currentDigits, int ***defaultDigits, int row, int col, int dimension, char playerInput,
                int *digitsShownCount) {
      if (!isDefaultDigit(*currentDigits, *defaultDigits, row, col)) {
            bool isHiddenSell = *(*(*currentDigits + row) + col) == 0 ? true : false;

            bool isPreviousDigitCorrect =
                isEnteredDigitCorrect(*currentDigits, dimension, row, col) && !isHiddenSell;

            *(*(*currentDigits + row) + col) = playerInput - '0';

            bool isCurrentDigitCorrect = isEnteredDigitCorrect(*currentDigits, dimension, row, col);

            if (isCurrentDigitCorrect && !isPreviousDigitCorrect)
                  (*digitsShownCount)++;
            else if (isPreviousDigitCorrect && !isCurrentDigitCorrect) {
                  (*digitsShownCount)--;
            }
      }
}
// checked
void clearSell(int ***currentDigits, int ***defaultDigits, int row, int col, int dimension,
               int *digitsShownCount) {
      if (!isDefaultDigit(*currentDigits, *defaultDigits, row, col)) {
            bool isHiddenSell = *(*(*currentDigits + row) + col) == 0 ? true : false;

            if (isEnteredDigitCorrect(*currentDigits, dimension, row, col) && !isHiddenSell)
                  (*digitsShownCount)--;

            *(*(*currentDigits + row) + col) = 0;
      }
}
// checked
void showEndOfGame(int filledSellsCount) {
      setColor(Color::GreenFg, Color::Default, "Congratulations!");
      setColor(Color::Default, Color::Default, " You have completed ");
      setColor(Color::CyanFg, Color::Default, "Sudoku");
      setColor(Color::Default, Color::Default, " and filled ");
      setColor(Color::MagentaFg, Color::Default, filledSellsCount);
      setColor(Color::Default, Color::Default, " empty cells!");
}
// checked
void showHint(int ***solvedMtrx, int ***defaultDigits, int dimension) {
      clear();
      drawField(*solvedMtrx, *defaultDigits, dimension, -1, -1);
      setColor(Color::BlackFg, Color::Default, "Press Enter to hide the hint.");
      getchar();
      getchar();
}
// checked
void showMenu(int digitsShownCount) {
      using std::cout, std::endl;

      cout << "\nm - show menu;" << endl;
      cout << "c - close menu;" << endl;
      cout << "Esc - close the game;\n" << endl;

      cout << "n - create new field;" << endl;
      cout << "r - reset all changes;" << endl;
      cout << "h - show hint;\n" << endl;

      cout << "w,a,s,d - moving;" << endl;
      cout << "1,2...9 - press if you want to change the cell;" << endl;
      cout << "0 - press if you want to clear inputed digit;\n" << endl;

      cout << "Remains to be filled out: ";
      setColor(Color::YellowFg, Color::Default, digitsShownCount);
      cout << ";\n" << endl;

      cout << "Field info: " << endl;
      setColor(Color::CyanFg, Color::Default, '*');
      cout << " or ";
      setColor(Color::CyanFg, Color::Default, '9');
      cout << " - current player position;" << endl;
      setColor(Color::BlackFg, Color::Default, '*');
      cout << " - empty cell;" << endl;
      setColor(Color::YellowFg, Color::Default, '9');
      cout << " - generated digit;" << endl;
      setColor(Color::RedFg, Color::Default, '9');
      cout << " - incorrect digit;" << endl;
      setColor(Color::GreenFg, Color::Default, '9');
      cout << " - correct digit;" << endl;
}
// checked
void showRules() {
      using std::cout, std::endl;

      cout << "Rules of the game \"Sudoku\":" << endl;

      cout << "------------------------------" << endl;

      cout << "- Sudoku is played over a 9x9 grid, divided \nto 3x3 sub grids called \"regions\";" << endl;
      cout << "- Sudoku begins with some of the grid cells \nalready filled with numbers;" << endl;
      cout << "- The object of Sudoku is to fill the other \nempty cells with numbers between 1 and 9 \n"
              "according the following guidelines:"
           << endl;

      cout << "1. Number can appear only once on each row;" << endl;
      cout << "2. Number can appear only once on each column;" << endl;
      cout << "3. Number can appear only once on each region;\n" << endl;

      cout << "Note: you can't change the ";
      setColor(Color::YellowFg, Color::Default, "generated");
      cout << " digit;" << endl;

      cout << "------------------------------\n" << endl;
}
// checked
void recreateDigits(int ***currentDigits, int ***defaultDigits, int ***solvedMtrx, int dimension,
                    int *playerPosX, int *playerPosY, int *digitsShownCount) {
      for (int i = 0; i < dimension; i++) wipeOfLine(solvedMtrx, dimension, i);

      initDigits(defaultDigits, solvedMtrx, currentDigits, dimension);

      refreshCurrentData(*defaultDigits, dimension, playerPosX, playerPosY, digitsShownCount);
}
// checked
void refreshField(int ***currentDigits, int **defaultDigits, int dimension, int *playerPosX, int *playerPosY,
                  int *digitsShownCount) {
      copyMtrx(defaultDigits, currentDigits, dimension);

      refreshCurrentData(defaultDigits, dimension, playerPosX, playerPosY, digitsShownCount);
}
// checked
void refreshCurrentData(int **defaultDigits, int dimension, int *playerPosX, int *playerPosY,
                        int *digitsShownCount) {
      const int MIDDLE = 4;

      *playerPosX = *playerPosY = MIDDLE;

      *digitsShownCount = getCountOfShownNumbers(defaultDigits, dimension);
}
// checked
bool isDefaultDigit(int **currentDigits, int **defaultDigits, int row, int col) {
      return (*(*(currentDigits + row) + col) == *(*(defaultDigits + row) + col)) &&
             (*(*(defaultDigits + row) + col) != 0);
}
// checked
void initDigits(int ***defaultDigitsPtr, int ***solvedDigitsPtr, int ***currentDigitsPtr, int dimension) {
      fillMtrxOfRandomDigitsFully(solvedDigitsPtr, dimension);

      copyMtrx(*solvedDigitsPtr, defaultDigitsPtr, dimension);

      hideAnyRandomDigits(defaultDigitsPtr);

      copyMtrx(*defaultDigitsPtr, currentDigitsPtr, dimension);
}
// checked
void copyMtrx(int **from, int ***to, int dimension) {
      for (int i = 0; i < dimension; i++) {
            for (int j = 0; j < dimension; j++) {
                  *(*(*to + i) + j) = *(*(from + i) + j);
            }
      }
}
// checked
void fillMtrxOfRandomDigitsFully(int ***mtrxPtr, int dimension) {
      int wipedLinesCount = 0;
      int lastWipedLine = 0;

      for (int i = 0; i < dimension; i++) {
            for (int j = 0; j < dimension; j++) {
                  int result = getRandomUnusedDigit(mtrxPtr, dimension, i, j);

                  if (result != -1) {
                        *(*(*mtrxPtr + i) + j) = result;
                  } else {
                        wipeOfLine(mtrxPtr, dimension, i);

                        if (++wipedLinesCount > 1 && lastWipedLine == i) {
                              for (int k = 0; k <= i; k++) wipeOfLine(mtrxPtr, dimension, k);

                              i = 0;

                              wipedLinesCount = 0;
                        }

                        j = -1;

                        lastWipedLine = i;
                  }
            }
      }
}
// checked
int getRandomUnusedDigit(int ***mtrxPtr, int dimension, int verticalIndex, int horizontalIndex) {
      int randomDigit = 1 + rand() % 9;
      int initialDigit = randomDigit;

      while (hasDigitInVerticalLine(*mtrxPtr, dimension, verticalIndex, horizontalIndex, randomDigit) ||
             hasDigitInHorizontalLine(*mtrxPtr, dimension, verticalIndex, horizontalIndex, randomDigit) ||
             hasDigitInCurrentSection(*mtrxPtr, randomDigit, horizontalIndex, verticalIndex)) {
            randomDigit = randomDigit < 9 ? randomDigit + 1 : 1;

            if (initialDigit == randomDigit) {
                  randomDigit = -1;

                  break;
            }
      }

      return randomDigit;
}
// checked
bool hasDigitInHorizontalLine(int **mtrx, int dimension, int verticalIndex, int horizontalIndex, int digit) {
      bool has = false;

      for (int i = 0; !has && i < dimension; i++) {
            if (i == horizontalIndex) continue;

            if (*(*(mtrx + verticalIndex) + i) == digit) has = true;
      }

      return has;
}
// checked
bool hasDigitInVerticalLine(int **mtrx, int dimension, int verticalIndex, int horizontalIndex, int digit) {
      bool has = false;

      for (int i = 0; !has && i < dimension; i++) {
            if (i == verticalIndex) continue;

            if (*(*(mtrx + i) + horizontalIndex) == digit) has = true;
      }

      return has;
}
// checked
bool hasDigitInCurrentSection(int **mtrx, int digit, int horizontalIndex, int verticalIndex) {
      const int SECTION_LEN = 3;

      bool has = false;

      for (int i = SECTION_LEN * (verticalIndex / SECTION_LEN);
           !has && i < SECTION_LEN + SECTION_LEN * (verticalIndex / SECTION_LEN); i++) {
            for (int j = SECTION_LEN * (horizontalIndex / SECTION_LEN);
                 !has && j < SECTION_LEN + SECTION_LEN * (horizontalIndex / SECTION_LEN); j++) {
                  if (i == verticalIndex && j == horizontalIndex) continue;

                  if (*(*(mtrx + i) + j) == digit) has = true;
            }
      }

      return has;
}
// checked
void wipeOfLine(int ***digitsPtr, int dimension, int indexOfLine) {
      const int DEFAULT_VALUE = 0;

      for (int i = 0; i < dimension; i++) {
            *(*(*digitsPtr + indexOfLine) + i) = DEFAULT_VALUE;
      }
}
// checked
void hideAnyRandomDigits(int ***digitsPtr) {
      const int LEFT_GROUND = 5, RIGHT_GROUND = 7;
      const int SECTIONS_COUNT = 9;
      const int SECTION_LEN = 3;

      for (int i = 0; i < SECTIONS_COUNT; i++) {
            int amountOfHiddenDigits = LEFT_GROUND + rand() % (RIGHT_GROUND - LEFT_GROUND + 1);

            hideDigitsInThisSection(digitsPtr, amountOfHiddenDigits, SECTION_LEN * (i / SECTION_LEN),
                                    SECTION_LEN * (i - SECTION_LEN * (i / SECTION_LEN)));
      }
}
// checked
void hideDigitsInThisSection(int ***digitPtr, int amountOfHiddenDigits, int startRow, int startCol) {
      const int SECTION_LEN = 3;

      while (amountOfHiddenDigits != 0) {
            int randomDigit = 1 + rand() % 9;

            bool isFound = false;
            while (!isFound) {
                  randomDigit = randomDigit < 9 ? randomDigit + 1 : 1;

                  for (int i = startRow; !isFound && i < startRow + SECTION_LEN; i++) {
                        for (int j = startCol; !isFound && j < startCol + SECTION_LEN; j++) {
                              if (*(*(*digitPtr + i) + j) == randomDigit) {
                                    *(*(*digitPtr + i) + j) = 0;

                                    isFound = true;

                                    amountOfHiddenDigits--;
                              }
                        }
                  }
            }
      }
}