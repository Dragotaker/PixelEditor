pipeline {
    agent any
    
    environment {
        DOCKER_REGISTRY = 'your-registry-url'
        DOCKER_IMAGE = 'pixel-editor'
        KUBE_CONFIG = credentials('kubeconfig')
    }
    
    stages {
        stage('Validate Credentials') {
            steps {
                script {
                    // Проверка наличия kubeconfig
                    if (!fileExists('kubeconfig')) {
                        error 'kubeconfig credentials not found in Jenkins'
                    }
                    
                    // Проверка наличия docker registry credentials
                    withCredentials([usernamePassword(credentialsId: 'docker-registry-credentials', 
                                                    usernameVariable: 'DOCKER_USER', 
                                                    passwordVariable: 'DOCKER_PASSWORD')]) {
                        echo "Docker registry credentials found"
                    }
                }
            }
        }
        
        stage('Build') {
            steps {
                script {
                    // Build the Docker image
                    docker.build("${DOCKER_REGISTRY}/${DOCKER_IMAGE}:${BUILD_NUMBER}")
                }
            }
        }
        
        stage('Test') {
            steps {
                echo 'Running tests...'
                // Add your test commands here
            }
        }
        
        stage('Push') {
            steps {
                script {
                    // Login to Docker registry
                    withCredentials([usernamePassword(credentialsId: 'docker-registry-credentials', 
                                                    usernameVariable: 'DOCKER_USER', 
                                                    passwordVariable: 'DOCKER_PASSWORD')]) {
                        sh "docker login ${DOCKER_REGISTRY} -u ${DOCKER_USER} -p ${DOCKER_PASSWORD}"
                    }
                    
                    // Push the image
                    docker.withRegistry("${DOCKER_REGISTRY}") {
                        docker.image("${DOCKER_REGISTRY}/${DOCKER_IMAGE}:${BUILD_NUMBER}").push()
                        docker.image("${DOCKER_REGISTRY}/${DOCKER_IMAGE}:${BUILD_NUMBER}").push('latest')
                    }
                }
            }
        }
        
        stage('Deploy') {
            steps {
                script {
                    // Write kubeconfig to file
                    writeFile file: 'kubeconfig', text: "${KUBE_CONFIG}"
                    
                    // Deploy to Kubernetes
                    sh """
                        export KUBECONFIG=kubeconfig
                        kubectl config use-context minikube
                        kubectl apply -f k8s/deployment.yaml
                        kubectl set image deployment/pixel-editor pixel-editor=${DOCKER_REGISTRY}/${DOCKER_IMAGE}:${BUILD_NUMBER}
                        kubectl rollout status deployment/pixel-editor
                    """
                }
            }
        }
    }
    
    post {
        always {
            // Cleanup
            sh 'docker rmi ${DOCKER_REGISTRY}/${DOCKER_IMAGE}:${BUILD_NUMBER} || true'
            sh 'rm -f kubeconfig'
        }
        success {
            echo 'Pipeline completed successfully!'
        }
        failure {
            echo 'Pipeline failed!'
        }
    }
} 