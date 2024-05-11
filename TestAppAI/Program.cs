using TestAppAI;

Console.WriteLine("Старт приложения. Нажми клавишу");

List<int> layers = new List<int> { 5, 4, 3, 2, 1 };
int firstLayerSize = 3;

List<TrainingData> data = new List<TrainingData>
{
    new TrainingData(){ Inbox = new List<double>() { 0, 0, 0 }, ExpectedAnswer = new List<double>(){ 1 } },
    new TrainingData(){ Inbox = new List<double>() { 0, 0, 1 }, ExpectedAnswer = new List<double>(){ 1 } },
    new TrainingData(){ Inbox = new List<double>() { 0, 1, 0 }, ExpectedAnswer = new List<double>(){ 1 } },
    new TrainingData(){ Inbox = new List<double>() { 1, 0, 0 }, ExpectedAnswer = new List<double>(){ 0 } },
    new TrainingData(){ Inbox = new List<double>() { 1, 0, 1 }, ExpectedAnswer = new List<double>(){ 1 } },
    new TrainingData(){ Inbox = new List<double>() { 1, 1, 0 }, ExpectedAnswer = new List<double>(){ 0 } },
    new TrainingData(){ Inbox = new List<double>() { 1, 1, 1 }, ExpectedAnswer = new List<double>(){ 1 } },
    new TrainingData(){ Inbox = new List<double>() { 0, 1, 1 }, ExpectedAnswer = new List<double>(){ 1 } },
};

Console.ReadLine();


Console.WriteLine("Начинаю создание нейроной сети");

NeuralNetworkDesigner neuralNetworkDesigner = new NeuralNetworkDesigner();
NeuralNetwork neuralNetwork = neuralNetworkDesigner.CreateNeuralNetwork(layers, firstLayerSize);

Console.WriteLine("Нейроная сеть создана");

while (true)
{
    Console.WriteLine("Ведите количество циклов обучения");
    string cycles = Console.ReadLine();
    neuralNetwork.StartTraining(data, int.Parse(cycles));
    Console.WriteLine("Обучение пройдено");

    foreach (TrainingData trainingData in data)
    {
        Console.WriteLine($"{trainingData.Inbox[0]}: наличие дождя. {trainingData.Inbox[1]} наличие зонта. {trainingData.Inbox[2]} наличие девушки");
        neuralNetwork.Start(trainingData.Inbox);

        Console.WriteLine($"ожидаемый ответ: {trainingData.ExpectedAnswer.Last()}. Ответ нейросети: {neuralNetwork.Reply}");
        Console.WriteLine();
    }

    Console.ReadLine();
}

