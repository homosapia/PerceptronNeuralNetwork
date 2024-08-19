using TestAppAI;
using static System.Net.Mime.MediaTypeNames;

Console.WriteLine("Старт приложения.");

Console.WriteLine("Полутить данный для обучения. Нажми клавишу");
Console.ReadLine();

string pathTraining = "C:\\Users\\plkmq\\OneDrive\\Рабочий стол\\DataAI\\Training";
DataCreationFactory dataCreation = new DataCreationFactory(pathTraining);
var images = dataCreation.ReadImages("\\train-images.idx3-ubyte");
var labels = dataCreation.ReadLabels("\\train-labels.idx1-ubyte");
List<TrainingData> data = dataCreation.GetTrainingData(images, labels);

Console.WriteLine("Получаю данные для тестирования");
string pathTest = "C:\\Users\\plkmq\\OneDrive\\Рабочий стол\\DataAI\\Test";
DataCreationFactory dataTestCreation = new DataCreationFactory(pathTest);
var imagesTest = dataTestCreation.ReadImages("\\t10k-images.idx3-ubyte");
var labelsTest = dataTestCreation.ReadLabels("\\t10k-labels.idx1-ubyte");
List<TrainingData> dataTests = dataTestCreation.GetTrainingData(imagesTest, labelsTest);


foreach (TrainingData dataItem in dataTests)
{
    // преобразуем массив в матрицу 28 на 28 пикселей
    int width = 28;
    int height = 28;
    byte[,] imageMatrix = new byte[height, width];
    for (int i = 0; i < height; i++)
    {
        for (int j = 0; j < width; j++)
        {
            imageMatrix[i, j] = (byte)dataItem.Inbox[i * width + j];
        }
    }

    // выводим изображение в консоль
    for (int i = 0; i < height; i++)
    {
        for (int j = 0; j < width; j++)
        {
            if (imageMatrix[i, j] == 0)
            {
                Console.Write(".");
            }
            else
            {
                Console.Write("#");
            }
        }
        Console.WriteLine();
    }

    if (!string.IsNullOrEmpty(Console.ReadLine()))
        break;
}


//List<TrainingData> data = new List<TrainingData>
//{
//    new TrainingData() { Inbox = new List<double> {0,0,0,0,0,0}, ExpectedAnswer = new List<double>(){0,0,0} },
//    new TrainingData() { Inbox = new List<double> {1,1,0,0,0,0}, ExpectedAnswer = new List<double>(){1,0,0} },
//    new TrainingData() { Inbox = new List<double> {0,0,0,0,1,1}, ExpectedAnswer = new List<double>(){0,1,0} },
//    new TrainingData() { Inbox = new List<double> {0,1,0,0,1,0}, ExpectedAnswer = new List<double>(){1,1,1} },
//    new TrainingData() { Inbox = new List<double> {1,0,0,0,0,0}, ExpectedAnswer = new List<double>(){0,0,0} },
//    new TrainingData() { Inbox = new List<double> {0,0,0,0,0,1}, ExpectedAnswer = new List<double>(){0,0,0} },
//    new TrainingData() { Inbox = new List<double> {0,1,1,0,0,0}, ExpectedAnswer = new List<double>(){1,0,0} },
//    new TrainingData() { Inbox = new List<double> {0,0,0,1,1,0}, ExpectedAnswer = new List<double>(){0,1,0} },
//    new TrainingData() { Inbox = new List<double> {1,0,0,0,0,1}, ExpectedAnswer = new List<double>(){1,1,1} },
//};

Console.WriteLine("Данные получены. Нажми клавишу");
Console.ReadLine();


Console.WriteLine("Начинаю создание нейроной сети");
List<int> layers = new List<int> { 196, 49, 10 };
int firstLayerSize = 784;
NeuralNetworkDesigner neuralNetworkDesigner = new NeuralNetworkDesigner();
NeuralNetwork neuralNetwork = neuralNetworkDesigner.CreateNeuralNetwork(layers, firstLayerSize);

Console.WriteLine("Нейроная сеть создана");

while (true)
{
    Console.WriteLine("Ведите количество циклов обучения");
    string cycles = Console.ReadLine();
    neuralNetwork.StartTraining(data, int.Parse(cycles));
    Console.WriteLine("Обучение пройдено");

    foreach(TrainingData dataTest in dataTests)
    {
        neuralNetwork.Start(dataTest.Inbox);
        var response = neuralNetwork.GetResponse();

        Console.WriteLine($"Ожидаемый ответ: {string.Join(", ", dataTest.ExpectedAnswer)}. Ответ: { string.Join(", ", response.Select(x => x.ToString("F10"))) }");
        Console.WriteLine();
        if (string.IsNullOrEmpty(Console.ReadLine()))
            continue;

        break;
    }

    Console.ReadLine();
}


