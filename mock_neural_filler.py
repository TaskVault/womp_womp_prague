import time
import random

def write_to_file(filename, frequency=60):
    interval = 1 / frequency
    with open(filename, 'w') as file:
        while True:
            if random.random() < 0.5:
                value = 0
            else:
                value = random.random()
            file.write(f"{value}\n")
            file.flush()  # Ensure the data is written to the file immediately
            time.sleep(interval)

if __name__ == "__main__":
    write_to_file("neuralInterpretations.txt")
