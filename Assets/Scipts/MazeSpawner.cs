using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

enum ValidAlgorithm
{
    Triangle,Rectangular
}
public class MazeSpawner : MonoBehaviour
{
    public GameObject cellPrefab;
    public  int sizeMaze;
    public CellScript[,] cellScripts;
    public int NumberOfOutputs=0;
    private int xCount;
    private int xWidth;
    private int yHeight;
    private ValidAlgorithm algorithm=ValidAlgorithm.Rectangular;
    private void Start()
    {
        
    }

    private void AddOutputs(CellScript[,] Cells)//Добавляем выходы
    {
        for (int Count = 0; Count < NumberOfOutputs; ++Count)
        {
            int RandomPoint = Random.Range(0, 4);
            switch (RandomPoint)
            {
                case 0://Выход слева
                    Destroy(Cells[0, Random.Range(0, sizeMaze - 1)].WallLeft);
                    break;
                
                case 1://Выход справа
                    Destroy(Cells[sizeMaze-1, Random.Range(0, sizeMaze - 1)].WallRight);
                    break;
                
                case 2://Выход сверху
                    Destroy(Cells[Random.Range(0, sizeMaze - 1),sizeMaze-1].WallTop);
                    break;
                
                case 3://Выход снизу
                    Destroy(Cells[Random.Range(0, sizeMaze - 1),0].WallBottom);
                    break;
                
                default:
                    Destroy(Cells[0, Random.Range(0, sizeMaze - 1)].WallLeft);
                    break;
            }
           
        }
    }
    
    public void SetValidAlgorithm(int value)
    {
        switch (value)
        {
            case 0 :
                algorithm = ValidAlgorithm.Rectangular;
                break;
            case 1:
                algorithm = ValidAlgorithm.Triangle;
                break;
            default:
                algorithm = ValidAlgorithm.Rectangular;
                break;
        }
    }
    
    public void GenerateMaze()
    {
        xWidth = sizeMaze; //xWidth нужен для того что бы можно было регулировать постройку по координатам для треугольника,не меняя текущих размеров.
        yHeight = sizeMaze;//Аналогично
        xCount = 0;
        transform.position.Set(100f, 100f, -10f);//Тут я пытался переместить камеру и настроить ее так что бы она смотрела на лабиринт без лишнего фона,я хз почему не работает.
        switch (algorithm)//Ищем нужный алгоритм 
        {
            case ValidAlgorithm.Rectangular:
                RecursiveAlgorithm();
                break;
            
            case ValidAlgorithm.Triangle :
                RecursiveAlgorithmTriangleMaze();
                break;
            
            default:
                RecursiveAlgorithm();
                break;
        }
    }
    private void RecursiveAlgorithmTriangleMaze() //Алгоритм Рандомизированный поиск в глубину Рекурсивная реализация в виде триугольника.
    {
        cellScripts = new CellScript[sizeMaze, sizeMaze];
        for (var y = 0; y < yHeight; ++y)
        {
            if (y == 0)//Если это начало постройки триугольника то заполняем по x полностью.
            {
                for (var x = 0; x < xWidth; ++x)
                {
                    cellScripts[x, y] = Instantiate(cellPrefab, new Vector2(x, y), Quaternion.identity)
                        .GetComponent<CellScript>();
                    cellScripts[x, y].y = y;
                    cellScripts[x, y].x = x;
                }
            }
            else//А иначе начинаем строить пирамиду ,добавляем одну x координату чтобы начать со следующей клетки.
            {
                xCount += 1;
                for (var x = xCount; x < xWidth; ++x)
                {
                    cellScripts[x, y] = Instantiate(cellPrefab, new Vector2(x, y), Quaternion.identity)
                        .GetComponent<CellScript>();
                    cellScripts[x, y].y = y;
                    cellScripts[x, y].x = x;
                }
            }

            xWidth -= 1;//Убираем координату для последней клетки.
        }

        AddOutputs(cellScripts);//Добавляем выходы

        var current = cellScripts[0, 0];//Устанавливаем начальную клетку
        current.Visited = true;//Помечаем ее как "Visited"

        var stack = new Stack<CellScript>();//Создаем новый стек,для того что бы откатывать шаги назад если у текущей клетки нету не посещенных соседей.
        NextStep(cellScripts, current, stack);//Делаем первый шаг.
    }

    public void RecursiveAlgorithm() //Алгоритм Рандомизированный поиск в глубину Рекурсивная реализация 
    {
        cellScripts = new CellScript[sizeMaze, sizeMaze];
        for (var x = 0; x < sizeMaze; ++x)//строим обычный квадрат sizeMaze x sizeMaze
        for (var y = 0; y < sizeMaze; ++y)
        {
            cellScripts[x, y] = Instantiate(cellPrefab, new Vector2(x, y), Quaternion.identity)
                .GetComponent<CellScript>();
            cellScripts[x, y].y = y;
            cellScripts[x, y].x = x;
        }
        
        AddOutputs(cellScripts);//Добавляем выходы
        
        var current = cellScripts[0, 0];//Устанавливаем начальную клетку
        current.Visited = true;//Помечаем ее как "Visited"

        var stack = new Stack<CellScript>();//Создаем новый стек,для того что бы откатывать шаги назад если у текущей клетки нету не посещенных соседей.
        NextStep(cellScripts, current, stack);//Делаем первый шаг.
    }

    private void NextStep(CellScript[,] cell, CellScript current, Stack<CellScript> stack)
    {
        do
        {
            current.GetComponent<SpriteRenderer>().color = Color.red;//Меняем цвет у текущей клетки.
            var inVisitedHeighbours = new List<CellScript>();//Записываем всех соседей текущей клетки в лист

            //--------------------Следующий код выглядит костыльным и ужасно отвратительным,но выглядит он так потому что он универсален для алгоритма "Рандомизированный поиск в глубину"
            if (current.x + 1 < sizeMaze)
                if (cell[current.x + 1, current.y] != null)
                    if (!cell[current.x + 1, current.y].Visited) //Сосед справа непосещался?
                        inVisitedHeighbours.Add(cell[current.x + 1, current.y]);


            if (current.x > 0)
                if (cell[current.x - 1, current.y] != null)
                    if (!cell[current.x - 1, current.y].Visited) //Сосед слева непосещался?
                        inVisitedHeighbours.Add(cell[current.x - 1, current.y]);


            if (current.y + 1 < sizeMaze)
                if (cell[current.x, current.y + 1] != null)
                    if (!cell[current.x, current.y + 1].Visited) //Сосед сверху непосещался?
                        inVisitedHeighbours.Add(cell[current.x, current.y + 1]);


            if (current.y > 0)
                if (cell[current.x, current.y - 1] != null)
                    if (!cell[current.x, current.y - 1].Visited) //Сосед снизу непосещался?
                        inVisitedHeighbours.Add(cell[current.x, current.y - 1]);


            if (inVisitedHeighbours.Count > 0)
                current = NeighborСhoice(current, stack, inVisitedHeighbours);
            else
                current = stack.Pop();
        } while (stack.Count != 0);
    }

    private CellScript NeighborСhoice(CellScript current, Stack<CellScript> stack, List<CellScript> inVisitedHeighbours)
    {
        var randomHeighbour = inVisitedHeighbours[Random.Range(0, inVisitedHeighbours.Count)];//Выбираем случайного соседа из списка.
        RemoveWalls(current, randomHeighbour);//Удаляем стены между текущей клеткой и клеткой соседа.
        randomHeighbour.Visited = true;//Помечаем клетку соседа как посещенную
        stack.Push(randomHeighbour);//Записываем клетку соседа в стек для возможности отката
        current = randomHeighbour;//Помечаем клетку соседа как текущую.
        return current;
    }

    private void RemoveWalls(CellScript current, CellScript randomHeighbour)//Тут думаю все понятно,можно было бы заменить на матрицу все эти if,но я так и не допер как.
    {
        if (randomHeighbour.y > current.y && randomHeighbour.x == current.x) //Сосед сверху 
        {
            Destroy(current.WallTop);
            Destroy(randomHeighbour.WallBottom);
        }

        if (randomHeighbour.y < current.y && randomHeighbour.x == current.x) //Сосед снизу
        {
            Destroy(current.WallBottom);
            Destroy(randomHeighbour.WallTop);
        }

        if (randomHeighbour.x > current.x && randomHeighbour.y == current.y) //Сосед справа
        {
            Destroy(current.WallRight);
            Destroy(randomHeighbour.WallLeft);
        }

        if (randomHeighbour.x < current.x && randomHeighbour.y == current.y) //Сосед слева
        {
            Destroy(current.WallLeft);
            Destroy(randomHeighbour.WallRight);
        }
    }
}