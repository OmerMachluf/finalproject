# Read the data file
library(tree)
treeModel = NULL

while (TRUE) {
    isTrainMode = try(read.csv("C:\\Users\\krist\\new\\temp\\mode.csv", as.is = T))

    if (isTrainMode[1, 1] == "-") {
        print("Mode wasn't selected yet")
    }
    else {
        if (isTrainMode[1, 1]) {
            x = try(read.csv("C:\\Users\\krist\\new\\temp\\training.csv"))
            if (nrow(x) > 0) {
                x$P_Time = as.double(x$P_Time)
                x$H_Time = as.numeric(x$H_Time)
                x$H_D_Time = as.factor(x$H_D_Time)

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
                treeModel = tree(as.factor(user) ~ ., trainData)

                # Run over testData
                treePred = predict(treeModel, testData, type = "class")

                # Get the difrences
                try(write(mean(treePred == testingTargetVector), "C:\\Users\\krist\\new\\temp\\prediction.txt"))

                print(mean(treePred == testingTargetVector))
            }
        }
        else {
            x = try(read.csv("C:\\Users\\krist\\new\\temp\\predict.csv"))
            if (nrow(x) > 0) {
                x$P_Time = as.double(x$P_Time)
                x$H_Time = as.numeric(x$H_Time)
                x$H_D_Time = as.factor(x$H_D_Time)

                arrData = x[sample(nrow(x), nrow(x)),]

                # Run over testData
                treePred = predict(treeModel, arrData, type = "class")

                arrPositiveClassificationVector = rep('PcOwner', nrow(x))

                # Get the difrences
                try(write(mean(treePred == arrPositiveClassificationVector), "C:\\Users\\krist\\new\\temp\\prediction.txt"))

                print(mean(treePred == arrPositiveClassificationVector))
            }
        }
    }

    Sys.sleep(0.5)
}













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