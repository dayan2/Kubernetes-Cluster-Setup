# Kubernetes Environment Setup
## _Multi-node - Cluster_

### MASTER NODE


```sh
#### Docker install
sudo apt-get remove -y docker docker-engine \
  docker.io containerd runc

curl -fsSL https://download.docker.com/linux/ubuntu/gpg \
  | sudo gpg --dearmor \
  -o /usr/share/keyrings/docker-archive-keyring.gpg
  
echo \
  "deb [arch=amd64 signed-by=/usr/share/keyrings/docker-archive-keyring.gpg] https://download.docker.com/linux/ubuntu $(lsb_release -cs) stable" \
  | sudo tee /etc/apt/sources.list.d/docker.list

sudo apt-get update

sudo apt-get install -y docker-ce docker-ce-cli containerd.io

sudo mkdir -p /etc/docker
cat <<EOF | sudo tee /etc/docker/daemon.json
{
    "exec-opts": ["native.cgroupdriver=systemd"],
    "log-driver": "json-file",
    "log-opts": {"max-size": "100m"},
    "storage-driver": "overlay2"
}
EOF

sudo systemctl restart docker
sudo systemctl enable docker
sudo usermod -aG docker $USER

sudo apt-get update
sudo apt-get install -y apt-transport-https ca-certificates \
  curl 
  
sudo curl -fsSLo /usr/share/keyrings/kubernetes-archive-keyring.gpg \
  https://packages.cloud.google.com/apt/doc/apt-key.gpg
  
# Add Kubernetes apt repository
echo "deb [signed-by=/usr/share/keyrings/kubernetes-archive-keyring.gpg] https://apt.kubernetes.io/ kubernetes-xenial main" \
  | sudo tee /etc/apt/sources.list.d/kubernetes.list

cd /etc/apt/sources.list.d/
## this file should exist -> kubernetes.list
cat kubernetes.list

cd
sudo apt-get update
sudo apt-get install -y kubelet kubeadm kubectl

#### Ensure swap is disabled
# See if swap is enabled
swapon --show

# Turn off swap
sudo swapoff -a

# Disable swap completely
sudo sed -i -e '/swap/d' /etc/fstab

systemctl status kubelet

# Add you VM Ip-address to apiserver-advertise-address
sudo kubeadm init --pod-network-cidr=10.244.0.0/16 --apiserver-advertise-address=172.31.4.124

mkdir -p $HOME/.kube
sudo cp -i /etc/kubernetes/admin.conf $HOME/.kube/config
sudo chown $(id -u):$(id -g) $HOME/.kube/config

#This should be done before worker nodes are joined.
sudo kubectl apply -f https://docs.projectcalico.org/v3.14/manifests/calico.yaml

sudo kubectl label node worker2 node-role.kubernetes.io/worker=worker
```

### WORKER NODE

```sh
sudo apt-get update

#### Docker install
sudo apt-get remove -y docker docker-engine \
  docker.io containerd runc

curl -fsSL https://download.docker.com/linux/ubuntu/gpg \
  | sudo gpg --dearmor \
  -o /usr/share/keyrings/docker-archive-keyring.gpg

echo \
  "deb [arch=amd64 signed-by=/usr/share/keyrings/docker-archive-keyring.gpg] https://download.docker.com/linux/ubuntu $(lsb_release -cs) stable" \
  | sudo tee /etc/apt/sources.list.d/docker.list

sudo apt-get update

sudo apt-get install -y docker-ce docker-ce-cli containerd.io

sudo mkdir -p /etc/docker
cat <<EOF | sudo tee /etc/docker/daemon.json
{
    "exec-opts": ["native.cgroupdriver=systemd"],
    "log-driver": "json-file",
    "log-opts": {"max-size": "100m"},
    "storage-driver": "overlay2"
}
EOF

sudo systemctl restart docker

sudo systemctl enable docker

sudo usermod -aG docker $USER

sudo apt-get update
sudo apt-get install -y apt-transport-https ca-certificates \
  curl 
  
sudo curl -fsSLo /usr/share/keyrings/kubernetes-archive-keyring.gpg \
  https://packages.cloud.google.com/apt/doc/apt-key.gpg

# Add Kubernetes apt repository
echo "deb [signed-by=/usr/share/keyrings/kubernetes-archive-keyring.gpg] https://apt.kubernetes.io/ kubernetes-xenial main" \
  | sudo tee /etc/apt/sources.list.d/kubernetes.list

cd /etc/apt/sources.list.d/
## this file should exist -> kubernetes.list
cat kubernetes.list

cd
sudo apt-get update
sudo apt-get install -y kubelet kubeadm kubectl

#### Ensure swap is disabled
# See if swap is enabled
swapon --show

# Turn off swap
sudo swapoff -a

# Disable swap completely
sudo sed -i -e '/swap/d' /etc/fstab

systemctl status kubelet

kubeadm join 172.31.5.142:6443 --token 1ohx7y.rgl8o79ny8bkmhzx \
        --discovery-token-ca-cert-hash sha256:8618bc4b90aabd386f284299dd3146e9d296e004a4927c035173ccb433cb4771

mkdir -p $HOME/.kube
## sudo cp -i /etc/kubernetes/admin.conf $HOME/.kube/config
sudo cp -i /etc/kubernetes/kubelet.conf $HOME/.kube/config
sudo chown $(id -u):$(id -g) $HOME/.kube/config 
  
```

## _Single-node - Cluster_
Provisioning Single-node Kubernetes Cluster using kubeadm on Ubuntu 20.04
```sh
sudo apt-get update
sudo apt-get install -y apt-transport-https ca-certificates \
  curl gnupg lsb-release
  
  # Remove all other versions of docker from your system
sudo apt-get remove -y docker docker-engine \
  docker.io containerd runc

# Add docker GPG key
curl -fsSL https://download.docker.com/linux/ubuntu/gpg \
  | sudo gpg --dearmor \
  -o /usr/share/keyrings/docker-archive-keyring.gpg

# Add docker apt repository
echo \
  "deb [arch=amd64 signed-by=/usr/share/keyrings/docker-archive-keyring.gpg] https://download.docker.com/linux/ubuntu $(lsb_release -cs) stable" \
  | sudo tee /etc/apt/sources.list.d/docker.list

# Fetch the package lists from docker repository
sudo apt-get update

# Install docker and containerd
sudo apt-get install -y docker-ce docker-ce-cli containerd.io

# Configure docker to use overlay2 storage and systemd
sudo mkdir -p /etc/docker
cat <<EOF | sudo tee /etc/docker/daemon.json
{
    "exec-opts": ["native.cgroupdriver=systemd"],
    "log-driver": "json-file",
    "log-opts": {"max-size": "100m"},
    "storage-driver": "overlay2"
}
EOF

# Restart docker to load new configuration
sudo systemctl restart docker

# Add docker to start up programs
sudo systemctl enable docker

# Allow current user access to docker command line
sudo usermod -aG docker $USER

# Add Kubernetes GPG key
sudo curl -fsSLo /usr/share/keyrings/kubernetes-archive-keyring.gpg \
  https://packages.cloud.google.com/apt/doc/apt-key.gpg

# Add Kubernetes apt repository
echo "deb [signed-by=/usr/share/keyrings/kubernetes-archive-keyring.gpg] https://apt.kubernetes.io/ kubernetes-xenial main" \
  | sudo tee /etc/apt/sources.list.d/kubernetes.list

# Fetch package list
sudo apt-get update

sudo apt-get install -y kubelet kubeadm kubectl

# Prevent them from being updated automatically
sudo apt-mark hold kubelet kubeadm kubectl

# See if swap is enabled
swapon --show

# Turn off swap
sudo swapoff -a

# Disable swap completely
sudo sed -i -e '/swap/d' /etc/fstab

sudo kubeadm init --pod-network-cidr=10.244.0.0/16

mkdir -p $HOME/.kube
sudo cp -i /etc/kubernetes/admin.conf $HOME/.kube/config
sudo chown $(id -u):$(id -g) $HOME/.kube/config

kubectl taint nodes --all node-role.kubernetes.io/master-

kubectl apply -f https://raw.githubusercontent.com/coreos/flannel/master/Documentation/kube-flannel.yml

# Add openebs repo to helm
helm repo add openebs https://openebs.github.io/charts

kubectl create namespace openebs

helm --namespace=openebs install openebs openebs/openebs

# Add bitnami repo to helm
helm repo add bitnami https://charts.bitnami.com/bitnami

helm install wordpress bitnami/wordpress \
  --set=global.storageClass=openebs-hostpath
```
