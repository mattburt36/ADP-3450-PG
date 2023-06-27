import matplotlib.pyplot as plt

def create_line_graph(data_file):
    # Read the data from the file
    with open(data_file, 'r') as file:
        data = file.read().split(',')

    # Convert the data to numeric values
    data = [float(value) for value in data]

    # Create x-coordinates based on the number of data points
    x = range(1, len(data) + 1)

    # Create the line graph
    plt.plot(x, data)
    plt.xlabel('T')
    plt.ylabel('mV')
    plt.title('Line Graph')
    plt.grid(True)

    # Display the graph
    plt.show()

# Specify the path to your data file
data_file = 'C:/Users/eng/Desktop/Workspace/CODE/sloan/ADP-3450-PG/ADP-3450/CodeSamples/py/data.txt'

# Call the function to create the line graph
create_line_graph(data_file)