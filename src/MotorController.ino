#define MotorPin 11 // Пин к которому подключаем мотор. Обязательно должен быть PWM.
#define BaudRate 9600 // Скорость передачи (9600/19200/115200).
int data = 50; // Скорость мотора на старте
void setup() {
  Serial.begin(BaudRate); // Скорость передачи.
  pinMode(MotorPin, OUTPUT); // Режим пина к которому подключается мотор - Выход.
  pinMode(13, OUTPUT); //Режим пина встроенного 13'ого светодиода - Выход.
}
void loop() {
  if(Serial.available()){ // Проверяем готовность последовательного порта.
    data = map(Serial.parseInt(),0,100,0,255); // Преобразуем входящие числа от 0 до 100 -> от 0 до 255.
    analogWrite(MotorPin, data); // Отправляем ШИМ сигнал на пин к которому подключен мотор.
  }
  else{ // Всё остальное время мигает 13 светодиод означающий что никакой передачи не идет в данный момент времени.
    digitalWrite(13, HIGH);
    delay(320);
    digitalWrite(13, LOW);
    delay(320);
    }
}
