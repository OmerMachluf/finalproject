DoAllOperations <- function()
    {
    isTrainMode = read.csv("C:/Users/krist/new/temp/mode.csv")
    if (isTrainMode[1, 1] == "-")
        {
        print("Mode wasn't selected yet")
        }
    else
       {
        if (isTrainMode[1, 1])
            {
            x = read.csv("C:/Users/krist/new/temp/training.csv")
            if (nrow(x) > 0)
                {
                arrData = x[sample(nrow(x), nrow(x)),]

                # Preparing the train and test index groups
                set.seed(2)
                classificationVector = arrData$user
                trainIndexes = sample(1:nrow(arrData), nrow(arrData) * 0.75)
                testIndexes = -trainIndexes

                # Get all relevant data by indexes
                trainData = arrData[trainIndexes,]
                testData = arrData[testIndexes,]

                # Get texting classification vector
                testingTargetVector = classificationVector[testIndexes]

                # Building the tree
                treeModel = tree(user ~ ., trainData)

                # Run over testData
                treePred = predict(treeModel, testData, type = "class")

                # Get the difrences
                write(as.character(treePred), "C:/Users/krist/new/temp/prediction.txt")
                }
            }
        else {
            x = read.csv("C:/Users/krist/new/temp/predict.csv")
            if (nrow(x) > 0) {
                # Run over testData
                treePred = predict(treeModel, x, type = "class")

                arrPositiveClassificationVector = rep('PcOwner', nrow(x))

                # Get the difrences
                write(treePred, "C:/Users/krist/new/temp/prediction.txt")
                print(mean(treePred == arrPositiveClassificationVector))
                }
            }
        }
    }
# Read the data file
library(tree)
treeModel = NULL
while (TRUE) {
    try(DoAllOperations())
}

Sys.sleep(0.5)
# Visualizing
plot(treeModel)
text(treeModel)

#Optimizations
cv_tree = cv.tree(treeModel, FUN = prune.misclass)
plot(cv_tree$size, cv_tree$dev, type = "b")

goodModel = prune.misclass(treeModel, best = 2)
plot(goodModel)
text(goodModel)

goodPred = predict(goodModel, testData, type = "class")
mean(goodPred != testingTargetVector)
