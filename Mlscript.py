import pandas
import sys
import csv, time, os
from pandas.plotting import scatter_matrix
import matplotlib.pyplot as plt
from sklearn import model_selection
from sklearn.metrics import classification_report
from sklearn.metrics import confusion_matrix
from sklearn.metrics import accuracy_score
from sklearn.linear_model import LogisticRegression
from sklearn.tree import DecisionTreeClassifier
from sklearn.neighbors import KNeighborsClassifier
from sklearn.discriminant_analysis import LinearDiscriminantAnalysis
from sklearn.naive_bayes import GaussianNB
from sklearn.svm import SVC

sys.__stdout__ = sys.stdout
basePath = "C:\\Users\\krist\\new\\temp\\"
selectedMode = basePath + "mode.csv"
trainingPath = basePath + "training.csv"
predictPath = basePath + "predict.csv"
predictionResultPath = basePath + "prediction.txt"
trainingResultPath = basePath + "training.txt"
scoring = 'accuracy'
tAlgorithm = 4
readNewData = True
validation_size = 0.01
seed = 7
sleep = 1.5
acc = "Not Ready.."
flag = False
predictionRecordsTimestamp = 0
trainingRecordsTimestamp = 0
if (tAlgorithm == 1):
        mlAlgo = LogisticRegression()
elif (tAlgorithm == 2):
        mlAlgo = LinearDiscriminantAnalysis()
elif (tAlgorithm == 3):
        mlAlgo = KNeighborsClassifier()
elif (tAlgorithm == 4):
        mlAlgo = DecisionTreeClassifier()
elif (tAlgorithm == 5):
        mlAlgo = GaussianNB()
elif (tAlgorithm == 6):
        mlAlgo = SVC()
while (True):
        #try:
                with open(selectedMode, 'r') as csvfile: # Check if we're training or testing
                        modeReader = csv.reader(csvfile, delimiter = ' ', quotechar = '|') 
                        for row in modeReader:
                                for value in row:
                                        if (value == "indication"):
                                                continue
                                        
                                        elif (value == "-"):
                                                acc = "Mode not selected yet"
                                                sleep = 3.5
                                                continue
                                        elif (value == "TRUE"): # we are testing
                                                sleep = 1.5
                                                newTrainingRecordsTimestamp = (os.stat(trainingPath)).st_mtime
                                                if (newTrainingRecordsTimestamp != trainingRecordsTimestamp):
                                                        print ("Identified change in recordings file.. updating new data")
                                                        trainingDataset = pandas.read_csv(trainingPath, header = None)
                                                        trainingDataset[[0, 1, 2]] = trainingDataset[[0, 1, 2]].astype(int)
                                                        trainingArray = trainingDataset.values
                                                        trainingArrayX = trainingArray[:,0:2]
                                                        trainingArrayY = trainingArray[:,2]
                                                        trainingArrayXT, trainingArrayXV, trainingArrayYT, trainingArraYV = model_selection.train_test_split(trainingArrayX, trainingArrayY, test_size=validation_size, random_state=seed)
                                                        mlAlgo.fit(trainingArrayXT, trainingArrayYT)
                                                        kfold = model_selection.KFold(n_splits=10, random_state=seed)
                                                        cv_results = model_selection.cross_val_score(mlAlgo, trainingArrayXT, trainingArrayYT, cv=kfold, scoring=scoring)
                                                        result = "%f (%f)" % (cv_results.mean(), cv_results.std())
                                                        acc = "Learning: " + result
                                                        trainingRecordsTimestamp = newTrainingRecordsTimestamp
                                                        flag = True
                                                with open(trainingResultPath, 'w') as trainingFile: # Check if we're training or testing
                                                        trainingFile.write(result)
                                        elif (value == "FALSE"):
                                                sleep = 1.5
                                                newPredictionRecordsTimestamp = (os.stat(predictPath)).st_mtime
                                                if (newPredictionRecordsTimestamp != predictionRecordsTimestamp):
                                                        print ("Identified change in predictions file.. updating new data")
                                                        predictDataset = pandas.read_csv(predictPath, header = None)
                                                        if predictDataset.size>5:
                                                                predictDataset[[0, 1, 2]] = predictDataset[[0, 1, 2]].astype(int)
                                                                predictArray = predictDataset.values
                                                                predictArrayX = predictArray[:,0:2]
                                                                predictArrayY = predictArray[:,2]
                                                                predictArrayXT, predictArrayXV, predictArrayYT, predictArraYV = model_selection.train_test_split(predictArrayX, predictArrayY, test_size=validation_size, random_state = seed)
                                                                if flag:
                                                                        predictions = mlAlgo.predict(predictArrayXT)
                                                                        score = "%f" % (accuracy_score(predictArrayYT, predictions))
                                                                        acc = "Prediction: " + score
                                                                        predictionRecordsTimestamp = newPredictionRecordsTimestamp
                                                                        with open(predictionResultPath, 'w') as predictionFile: # Check if we're training or testing
                                                                                predictionFile.write(score)
                                                                else:
                                                                        predictionRecordsTimestamp = newPredictionRecordsTimestamp
                                                                        acc = "Please go through learning "
                                                        else:
                                                                predictionRecordsTimestamp = newPredictionRecordsTimestamp
                                                                acc = "Not enough records to predict.. hold"
        #except Exception:
                #acc = "Ohhh shit"
                                print(acc)
                                time.sleep(sleep)
        #time.sleep(sleep)

# shape
# print(dataset.shape)
# head
# print(dataset.head(20))
# descriptions
# print(dataset.describe())


#validation_size = 0.20
#seed = 7
#X_train, X_validation, Y_train, Y_validation = model_selection.train_test_split(X, Y, test_size=validation_size, random_state=seed)
#X2_train, X2_validation, Y2_train, Y2_validation = model_selection.train_test_split(X2, Y2, test_size=validation_size, random_state=seed)

#models = []
#models.append(('LR', LogisticRegression()))
#models.append(('LDA', LinearDiscriminantAnalysis()))
#models.append(('KNN', KNeighborsClassifier()))
#models.append(('CART', DecisionTreeClassifier()))
#models.append(('NB', GaussianNB()))
#models.append(('SVM', SVC()))
# evaluate each model in turn
#results = []
#names = []
#for name, model in models:
#	kfold = model_selection.KFold(n_splits=10, random_state=seed)
#	cv_results = model_selection.cross_val_score(model, X_train, Y_train, cv=kfold, scoring=scoring)
#	results.append(cv_results)
#	names.append(name)
#	msg = "%s: %f (%f)" % (name, cv_results.mean(), cv_results.std())
#	print(msg)

#print(confusion_matrix(Y2_validation, predictions))
#print(classification_report(Y2_validation, predictions))
